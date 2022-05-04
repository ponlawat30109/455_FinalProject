var mongo = require("mongodb").MongoClient;
// var url = "mongodb://localhost:27017";
// var url = "mongodb://25.111.112.228:27017";

var url = "mongodb+srv://aom6458:yjKmUJyG0ivTjnzK@cluster0.kx7zo.mongodb.net/test?authSource=admin&replicaSet=atlas-gub76x-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true";
// var url = "mongodb+srv://test:test@cluster0.kx7zo.mongodb.net/test?authSource=admin&replicaSet=atlas-gub76x-shard-0&readPreference=primary&appname=MongoDB%20Compass&ssl=true";
var websocket = require("ws");

var callbackInitServer = () => {
  console.log("Fralyst Server is running.");
};

var wss = new websocket.Server({ port: 5500 }, callbackInitServer);

var wsList = [];

var roomMap = new Map();

wss.on("connection", (ws) => {
  {
    ws.on("message", (data) => {
      var toJSON = JSON.parse(data);
      var userCheck = JSON.parse(toJSON.data);
      mongo.connect(url, { useUnifiedTopology: true }, (err, result) => {
        if (err) throw err;
        // console.log("Connect to Database");
        var selectDB = result.db("GI455");
        switch (toJSON.eventName) {
          case "Login":
            // Register(selectDB, "1", "1", "test");
            Login(ws, selectDB, userCheck.username, userCheck.password);
            break;
          case "Register":
            Register(
              ws,
              selectDB,
              userCheck.username,
              userCheck.password,
              userCheck.playername,
              userCheck.hero
            );
            break;
          default:
            console.log("Error case");
        }
      });

      //   return toJSON;
    });
  }

  console.log("client connected.");
  wsList.push(ws);

  ws.on("close", () => {
    // LeaveRoom(ws, (status, roomKey) => {
    //   if (status === true) {
    //     if (roomMap.get(roomKey).wsList.size <= 0) {
    //       roomMap.delete(roomKey);
    //     }
    //   }
    // });
  });
});

var Login = (ws, db, _username, _password) => {
  var newData = {
    username: _username,
    password: _password,
  };

  var playerData = "playerData";

  var query = {
    username: _username,
    password: _password,
  };

  db.collection(playerData).find(query).toArray((err, result) => {
      if (err) {
        console.log(false);
      } else {
        console.log(result);
        if (result) {
          var resultData = {
            eventName: "Login",
            data: JSON.stringify(result[0]),
            status: "true",
          };
          ws.send(JSON.stringify(resultData));
        } else {
          var resultData = {
            eventName: "Login",
            data: JSON.stringify(result[0]),
            status: "false",
          };
          ws.send(JSON.stringify(resultData));
        }
      }
    });
};

var Register = (ws, db, _username, _password, _playername, _heroname) => {
  var newData = {
    username: _username,
    password: _password,
    playername: _playername,
    heroname: _heroname,
  };

  var playerData = "playerData";

  var query = {
    username: _username,
    playername: _playername,
  };

  db.collection(playerData)
    .find(query)
    .toArray((err, result) => {
      if (err) {
        console.log(`Register Err : ${err}`);
      } else {
        temp = JSON.stringify(result[0])
        console.table(temp);
        if (temp == null) {
          console.log("username is not exist");
          db.collection(playerData).insertOne(newData, (err, result) => {
            if (err) {
              console.log(err);
            } else {
              if (result.result.ok == 1) {
                console.log("Register success");
              } else {
                console.log("Register fail");
              }
            }
          });

          var resultData = {
            eventName: "Register",
            data: temp,
            status: "true",
          };
          ws.send(JSON.stringify(resultData));
        } else {
          console.log("username is exist");
          var resultData = {
            eventName: "Register",
            data: temp,
            status: "false",
          };
          ws.send(JSON.stringify(resultData));
        }
      }
    });
};
