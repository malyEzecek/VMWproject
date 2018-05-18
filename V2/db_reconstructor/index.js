const fs = require('fs');
const cors = require('cors');
const express = require('express');
const app = express();
const bodyParser = require('body-parser');
const fetch = require('node-fetch');
const http = require('http').Server(app);
const io = require('socket.io')(http);
const port = 3001;

app.use(express.static('public'))
app.use(cors());
app.use(bodyParser.json());

const URL = 'http://localhost:3000/search';
const HEADERS = {"Content-Type": "application/json"};

const DICTIONARY = fs.readFileSync('./dict_short.txt').toString().split("\n");
const MAX_VALUES = [5, 4, 4, 4, 1, 1, 1, 1];

io.on('connection', socket => {
  new DBReconstructor(socket);
})

http.listen(port, () => {
  console.log("Server listening on port " + port)
});

class DBReconstructor {
  constructor(socket) {
    this.maxResults = 100;
    this.retrieved = [];
    this.retrievedIDs = new Set();
    this.socket = socket;
    this.wordIndex = 0;

    this.socket.on("stop", () => {
      if (this.running)
        this.stop();
    });

    this.socket.on("disconnect", () => {
      console.log("Client disconnected");
      if (this.running)
        this.stop();
    });

    this.socket.on("run", value => {
      if (value === this.running)
        return;
      if (value)
        this.run();
      else
        this.stop();
    });
  }

  run() {
    console.log("Starting reconstruction.");
    this.clear();
    this.wordIndex = 0;
    this.lastSize = 0;
    this.timeThen = Date.now();
    this.running = true;
    
    this.nextWord(0).then(() => {
      this.stop();
    })
  }

  stop() {
    this.running = false;
    fs.writeFileSync("./public/data.json", JSON.stringify(this.retrieved));
    this.socket.emit("done");
    console.log("Stopping reconstruction.");
  }

  findCars(query, vector) {
    const body = JSON.stringify({query: query, size: this.maxResults, vector: vector});
    return fetch(URL, {method: "POST", headers: HEADERS, body: body}).then(res => res.json());
  }

  find(query, vector, index) {
    return this.findCars(query, vector).then(result => {
      if (!this.running)
        return Promise.resolve();
      
      result.items.forEach(car => {
        if (!this.retrievedIDs.has(car._id)) {
          this.retrievedIDs.add(car._id);
          this.retrieved.push(car);
        }
      });
  
      if (this.retrieved.length !== this.lastSize) {
        const timestamp = (Date.now() - this.timeThen) / 1000;
        var log =  query + "," + timestamp + "," + this.retrieved.length + "\n";
        this.newRecord({timestamp: timestamp, size: this.retrieved.length, query: query});
        console.log(log);
        fs.appendFile('./public/results.csv', log, function (err) {
          if (err) return console.log(err);
        });
      }
  
      this.lastSize = this.retrieved.length;
  
      if (!result.isWhole && index < 8) {
        var allPromises = [];
  
        for (var opt = 0; opt <= MAX_VALUES[index]; opt++) {
          var cpy = [...vector]
          cpy[index] = opt;
          allPromises.push(this.find(query, cpy, index + 1));
        }
  
        return Promise.all(allPromises);
      } else if (index == 8)
        console.log("unreachable");
  
      return Promise.resolve();
    }, console.error)
  }

  nextWord(i) {
    if (i > DICTIONARY.length - 1 || !this.running)
      return;
    
    var word = DICTIONARY[i].trim();
  
    return this.find(word, Array(8), 0).then(() => this.nextWord(i + 1));
  }

  clear() {
    this.retrieved.length = [];
    this.retrievedIDs.clear();
    try {
      fs.unlinkSync("./public/results.csv");
    } catch(e) {

    }
  }

  newRecord(value) {
    this.socket.emit("record", value);
  }
}