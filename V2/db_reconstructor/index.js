const fs = require('fs');
const cors = require('cors');
const express = require('express');
const bodyParser = require('body-parser');
const fetch = require('node-fetch');

const app = express();
const port = 3001;

app.use(express.static('public'))
app.use(cors());
app.use(bodyParser.json());

const URL = 'http://localhost:3000/search';
const HEADERS = {"Content-Type": "application/json"};

const DICTIONARY = fs.readFileSync('./dict_short.txt').toString().split("\n");
const MAX_VALUES = [5, 4, 4, 4, 1, 1, 1, 1];

class DBReconstructor {
  constructor(_maxResults) {
    this.maxResults = _maxResults;
  }

  run() {
    console.log("Starting reconstruction.");
    this.lastSize = 0;
    this.timeThen = Date.now();
    this.retrieved = [];
    this.retrievedIDs = new Set();
    this.running = true;
    
    this.nextWord(0);
  }

  stop() {
    console.log("Stopping reconstruction.");
    this.running = false;
  }

  findCars(query, vector) {
    const body = JSON.stringify({query: query, size: this.maxResults, vector: vector});
    return fetch(URL, {method: "POST", headers: HEADERS, body: body}).then(res => res.json());
  }

  find(query, vector, index) {
    return this.findCars(query, vector).then(result => {
      result.items.forEach(car => {
        if (!this.retrievedIDs.has(car._id)) {
          this.retrievedIDs.add(car._id);
          this.retrieved.push(car);
        }
      });
  
      if (this.retrieved.length !== this.lastSize) {
        var log =  query + "," + (Date.now() - this.timeThen) / 1000 + "," + this.retrieved.length + "\n";
        console.log(log);
        fs.appendFile('./results.csv', log, function (err) {
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
    if (i > DICTIONARY.length - 1)
      return;
    
    var word = DICTIONARY[i].trim();
  
    this.find(word, Array(8), 0).then(() => this.nextWord(i + 1));
  }
}

app.listen(port, () => {
  console.log("Server listening on port " + port)

  var rec = new DBReconstructor(100);
  rec.run();
});
