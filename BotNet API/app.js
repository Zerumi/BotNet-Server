const express = require('express');
const bodyParser = require('body-parser');
const logger = require('morgan');
const fs = require('fs');
const path = require('path');
const app = express();
const PORT = process.env.PORT || 3000;
const NODE_ENV = process.env.NODE_ENV || 'development';
var ip = require('./ip.json');
const messages = require('./messages.json');
app.set('port', PORT);
app.set('env', NODE_ENV);
app.use(logger('tiny'));
app.use(bodyParser.text());
app.get('/api/v1/ip', (req, res, next) => {
    try {
        const playerStats = ip;
        res.json(playerStats);
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
        fs.readFile('ip.json', 'utf8', function readFileCallback(err, data) {
            if (err) {
                console.log(err);
            } else {
                var obj = JSON.parse(data); //now it an object
                obj.push(newIP); //add some data
                var json = JSON.stringify(obj); //convert it back to json
                fs.writeFile('ip.json', json, 'utf8', (error) => { console.log(error) }); // write it back 
            }
        });
        res.status(201).json(newIP);
    } catch (e) {
        next(e);
    }
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