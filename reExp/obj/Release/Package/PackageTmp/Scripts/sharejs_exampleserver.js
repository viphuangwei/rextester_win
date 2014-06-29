//#!/usr/bin/env node

//require('coffee-script');
//var connect = require('connect'),
//    sharejs = require('../src');

//var server = connect(
//      //connect.logger(),
//      connect.static(__dirname)
//    );

//var options = {
//  db: {type: 'none'},
//  browserChannel:{cors: "*"}
//};

//// Lets try and enable redis persistance if redis is installed...
//try {
//  require('redis');
//  options.db = {type: 'redis'};
//} catch (e) {console.log("redis could not be started")}

//var today = new Date();
//console.log(today.getFullYear()+"-"+(today.getMonth()+1)+"-"+today.getDate()+": ShareJS example server v" + sharejs.version);
//console.log("Options: ", options);

//// Attach the sharejs REST and Socket.io interfaces to the server
//sharejs.server.attach(server, options);

//server.listen(8000);
//console.log('Server running at http://127.0.0.1:8000/');

//process.title = 'sharejs_exampleserver'
//process.on('uncaughtException', function (err) {
//  console.error('An error has occurred. Please file a ticket here: https://github.com/josephg/ShareJS/issues');
//  console.error('Version ' + sharejs.version + ': ' + err.stack);
//});


///*
//#!/usr/bin/env node

//require('coffee-script');
//var connect = require('connect'),
//    sharejs = require('../src');

//var server = connect(
//      //connect.logger(),
//      connect.static(__dirname),
//      connect.router(function (app) {
//                app.get('/*', function(req, res, next) {
//                    res.setHeader('Access-Control-Allow-Origin', '*');
//                    res.setHeader('Access-Control-Allow-Headers', 'X-Requested-With');
//                    res.setHeader('X-XSS-Protection', '0');
//                    next();
//                });
//          })
//    );

//var options = {
//  db: {type: 'none'},
//  browserChannel:{cors: "*"}
//};


//// Lets try and enable redis persistance if redis is installed...
//try {
//  require('redis');
//  options.db = {type: 'redis'};
//} catch (e) {console.log("redis could not be started")}

//var today = new Date();
//console.log(today.getFullYear()+"-"+(today.getMonth()+1)+"-"+today.getDate()+": ShareJS example server v" + sharejs.version);
//console.log("Options: ", options);

//// Attach the sharejs REST and Socket.io interfaces to the server
//sharejs.server.attach(server, options);

//server.listen(8000);
//console.log('Server running at http://127.0.0.1:8000/');

//process.title = 'sharejs_exampleserver'
//process.on('uncaughtException', function (err) {
//  console.error('An error has occurred. Please file a ticket here: https://github.com/josephg/ShareJS/issues');
//  console.error('Version ' + sharejs.version + ': ' + err.stack);
//});


//*/
///*
//#!/usr/bin/env node
//// This is a simple example sharejs server which hosts the sharejs
//// examples in examples/.
////
//// It demonstrates a few techniques to get different application behaviour.

//require('coffee-script');
//var connect = require('connect'),
//        sharejs = require('../src'),
//        hat = require('hat').rack(32, 36);

//var argv = require('optimist').
//        usage("Usage: $0 [-p portnum]").
//        default('p', 8000).
//        alias('p', 'port').
//        argv;

//var server = connect(
//        connect.favicon(),
//        connect.static(__dirname + '/../examples'),
//        connect.router(function (app) {
//                var renderer = require('../examples/_static');
//                app.get('/static/:docName', function(req, res, next) {
//                        var docName;
//                        docName = req.params.docName;
//                        renderer(docName, server.model, res, next);
//                });

//                var wiki = require('../examples/_wiki');
//                app.get('/wiki/?', function(req, res, next) {
//                        res.writeHead(301, {location: '/wiki/Main'});
//                        res.end();
//                });

//                app.get('/wiki/:docName', function(req, res, next) {
//                        var docName;
//                        docName = req.params.docName;
//                        wiki(docName, server.model, res, next);
//                });

//                app.get('/pad/?', function(req, res, next) {
//                        var docName;
//                        docName = hat();
//                        res.writeHead(303, {location: '/pad/pad.html#' + docName});
//                        res.write('');
//                        res.end();
//                });

//                app.get('/?', function(req, res, next) {
//                        res.writeHead(302, {location: '/index.html'});
//                        res.end();
//                });
//        })
//);

//var options = {
//  db: {type: 'none'},
//  auth: function(client, action) {
//                // This auth handler rejects any ops bound for docs starting with 'readonly'.
//    if (action.name === 'submit op' && action.docName.match(/^readonly/)) {
//      action.reject();
//    } else {
//      action.accept();
//    }
//  },
//  browserChannel:{cors: "*"}
//};

//// Lets try and enable redis persistance if redis is installed...
//try {
//  require('redis');
//  options.db = {type: 'redis'};
//} catch (e) {console.log("redis could not be started")}

//console.log("ShareJS example server v" + sharejs.version);
//console.log("Options: ", options);

//var port = argv.p;

//// Attach the sharejs REST and Socket.io interfaces to the server
//sharejs.server.attach(server, options);

//server.listen(port);
//console.log("Demos running at http://localhost:" + port);

//process.title = 'sharejs'
//process.on('uncaughtException', function (err) {
//  console.error('An error has occurred. Please file a ticket here: https://github.com/josephg/ShareJS/issues');
//  console.error('Version ' + sharejs.version + ': ' + err.stack);
//});
//*/