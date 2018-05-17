const fs = require('fs');
const cors = require('cors');
const express = require('express');
const bodyParser = require('body-parser');
const MongoClient = require('mongodb').MongoClient;

const generator = require('./js/generator');
const finder = require('./js/find');

const app = express();
const port = 3000;
const DB_URL = "mongodb://localhost:27017/";
let DB; // declare database
let collection;

app.use(express.static('public'))
app.use(cors());
app.use(bodyParser.json());

MongoClient.connect(DB_URL, { useNewUrlParser: true }, function(err, db) {
    if (err) throw err;
    DB = db;
    collection = DB.db("CarShop").collection("Cars");
    console.log("Connected to database!");
    // Tohle je soucast crackovaciho serveru
    /*
    const DICTIONARY = fs.readFileSync('./dict_short.txt').toString().split("\n");
    const MAX_VALUES = finder.MAX_VALUES;
  
    var retrievedIDs = new Set();
    var retrieved = [];
  
    var lastSize = 0;
    var wordLength = 0;
    var timeThen = Date.now();
  
    var find = (query, vector, index) => {
      return findCars(collection, query, vector, RESULTS_RETURNED).then(result => {
        for (let car of result.items) {
          if (!retrievedIDs.has(car._id)) {
            retrievedIDs.add(car._id);
            retrieved.push(car);
          }
        }
  
        if (retrieved.length !== lastSize) {
          var log =  query + "," + (Date.now() - timeThen) / 1000 + "," + retrieved.length + "\n";
          console.log(log);
          fs.appendFile('./results.csv', log, function (err) {
            if (err) return console.log(err);
         });
        }
  
        lastSize = retrieved.length;
  
        if (!result.isWhole && index < 8) {
          var allPromises = [];
  
          for (var opt = 0; opt <= MAX_VALUES[index]; opt++) {
            var cpy = [...vector]
            cpy[index] = opt;
            allPromises.push(find(query, cpy, index + 1));
          }
  
          return Promise.all(allPromises);
        } else if (index == 8)
          console.log("nedosazitelne");
  
        return Promise.resolve();
      }, console.log)
      
    }
  
    function next(i) {
      if (i > DICTIONARY.length - 1)
        return;
      
      var word = DICTIONARY[i].trim();
  
      find(word, Array(8), 0).then(() => next(i + 1));
    }
    next(0);
    */
  });

app.post('/search', (req, res) => {
  const body = req.body;
  if (!body.query || !body.vector || !body.size)
    res.status(400).send(JSON.stringify({error: "Bad request!"}));
  else
    finder.findCars(collection, body.query, body.vector, body.size)
    .then(results => res.send(JSON.stringify(results)))
})

app.listen(port, () => console.log("Server listening on port " + port));
  