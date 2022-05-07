var express = require("express")
    mongoose = require("mongoose")
    bodyParser = require("body-parser");

var db;
if(process.env.ENV == "Test"){
    db = mongoose.connect("mongodb://localhost/bookAPI_test");
}
else{
    db = mongoose.connect("mongodb://localhost/bookAPI");
}

var app = express();
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended:true}));

//Route Setup
var Book = require("./models/bookModel");
bookRouter = require("./routes/bookRoutes")(Book);
app.use("/api/books", bookRouter);

app.get("/", function(req, res){
    res.send("Welcome to the API - Live from Gulp");
});

//Server Start
var port = process.env.port || 3000;
app.listen(port, function(){
    console.log("Running on port: " + port);
});

module.exports = app;