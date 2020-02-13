// server.js
// where your node app starts

// init project
const express = require("express");
const logger = require("morgan");
const bodyParser = require("body-parser");
const fs = require("fs");
const PORT = process.env.PORT || 3000;
const NODE_ENV = process.env.NODE_ENV || "development";
const app = express();
var prescorenet = require("./prescorenet-zip.json");

app.set("port", PORT);
app.set("env", NODE_ENV);
app.use(logger("tiny"));
app.use(bodyParser.text());

app.get("/", function (request, response) {
    response.json(
        "mineweb-hackserver base version v.1.2.2 (29.01.20); database version 1.34.449.1.9 (29.01.20)"
    );
    response.end();
});

app.get("/scripts/prescorenet.zip", (req, res, next) => {
    try {
        res.json(prescorenet);
        res.end();
    } catch (e) {
        next(e);
    }
});

app.delete("/scripts/prescorenet.zip", (req, res, next) => {
    prescorenet = [];
    fs.writeFileSync("prescorenet-zip.json", JSON.stringify(prescorenet));
    res.end();
});

app.post("/scripts/prescorenet.zip", (req, res, next) => {
    prescorenet = prescorenet.concat(JSON.parse(req.body));
    fs.writeFileSync("prescorenet-zip.json", JSON.stringify(prescorenet));
    res.end();
});

app.use((req, res, next) => {
    const err = new Error(`${req.method} ${req.url} Not Found`);
    err.status = 404;
    next(err);
});
app.use((err, req, res, next) => {
    console.error(err);
    res.status(err.status || 500);
    res.json({
        error: {
            message: err.message
        }
    });
});
app.listen(PORT, () => {
    console.log(
        `Express Server started on Port ${app.get(
            "port"
        )} | Environment : ${app.get("env")}`
    );
});
