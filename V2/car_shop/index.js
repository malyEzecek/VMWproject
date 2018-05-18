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
  