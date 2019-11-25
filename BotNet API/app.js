const express = require('express');
const bodyParser = require('body-parser');
const logger = require('morgan');
const fs = require('fs');
const path = require('path');
const app = express();
const PORT = process.env.PORT || 3000;
const NODE_ENV = process.env.NODE_ENV || 'development';
const ip = require('./ip.json');
var messages = require('./messages.json');
app.set('port', PORT);
app.set('env', NODE_ENV);
app.use(logger('tiny'));
app.use(bodyParser.text());
app.get('/api/v1/ip', (req, res, next) => {
    try {
        const playerStats = require('./ip.json');
        res.json(playerStats);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.get('/api/v1/messages', (req, res, next) => {
    try {
        res.json(messages.length);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.get('/api/v1/messages/:id', (req, res, next) => {
    try {
        const playerStats = messages.find(player => player.id === Number(req.params.id));
        if (!playerStats) {
            const err = new Error('message not found');
            err.status = 404;
            throw err;
        }
        res.json(playerStats);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.post('/api/v1/ip', (req, res, next) => {
    try {
        const newIP = {
            id: JSON.parse(req.body).id,
            ip: JSON.parse(req.body).ip,
        };
        ip.push(newIP);
        var sip = JSON.stringify(ip);
        fs.writeFileSync("ip.json",sip);
        res.status(201).json(newIP);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.post('/api/v1/messages', (req, res, next) => {
    try {
        const newMessage = {
            id: JSON.parse(req.body).id,
            command: JSON.parse(req.body).command,
            ip: JSON.parse(req.body).ip
        };
        messages.push(newMessage);
        var smessages = JSON.stringify(messages);
        fs.writeFileSync("messages.json",smessages);
        res.status(201).json(newMessage);
    } catch (e) {
        next(e);
    }
    res.end();
});
app.delete('/api/v1/messages', (req, res, next) => {
    messages = [];
    
    res.end();
});
app.get('/', function (req, res) {
    res.send('hello world');

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
            message: err.message,
        },
    });
});
app.listen(PORT, () => {
    console.log(
        `Express Server started on Port ${app.get(
            'port'
        )} | Environment : ${app.get('env')}`
    );
});