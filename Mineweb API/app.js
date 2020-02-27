// server.js
// where your node app starts

// init project
const express = require("express");
const logger = require("morgan");
const bodyParser = require("body-parser");
const fs = require("fs");
const glob = require("glob");
const PORT = process.env.PORT || 3000;
const NODE_ENV = process.env.NODE_ENV || "development";
const app = express();

app.set("port", PORT);
app.set("env", NODE_ENV);
app.use(logger("tiny"));
app.use(bodyParser.text());

app.get("/", function (request, response) {
    response.json(
        "mineweb-hackserver base version v.1.5.1.42 (27.02.20); database version 1.78.4.3 (27.02.20)"
    );
    response.end();
});

app.get("/scripts/:script", (req, res, next) => {
    var file;
    glob("./scripts/*.json", (err, files) => {
        try {
            if (err) {
                throw err;
            }
            file = files.find(x => x == "./scripts/" + req.params.script + ".json");
            fs.readFile(file, 'utf8', (err, data) => {
                if (err) {
                    throw err;
                }
                res.json(JSON.parse(data)).end();
            });
        }
        catch (e) {
            next(e);
        }
    });
});

app.post("/scripts/:script", (req, res, next) => {
    var file;
    glob("./scripts/*.json", (err, files) => {
        try {
            if (err) {
                throw err;
            }
            file = files.find(x => x == "./scripts/" + req.params.script + ".json");
            if (!file) {
                fs.appendFile("./scripts/" + req.params.script + ".json", "[]", () => { });
            }
            fs.readFile(file, 'utf8', (err, data) => {
                if (err) {
                    throw err;
                }
                var firststr = JSON.parse(data);
                var finalstr = firststr.concat(JSON.parse(req.body));
                fs.writeFileSync(file, JSON.stringify(finalstr));
                res.end();
            });
        }
        catch (e) {
            next(e);
        }
    });
});

app.delete("/scripts/:script", (req, res, next) => {
    glob("./scripts/*.json", (err, files) => {
        try {
            if (err) {
                throw err;
            }
            file = files.find(x => x == "./scripts/" + req.params.script + ".json");
            fs.readFile(file, 'utf8', (err, data) => {
                if (err) {
                    throw err;
                }
                fs.writeFileSync(file, JSON.stringify([]));
                res.end();
            });
        }
        catch (e) {
            next(e);
        }
    });
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
