import { setTimeout, setInterval, clearInterval, clearTimeout } from "system/timer"
import { postMessage, parentPort  } from "system/worker"
(function () {

    parentPort.onmessage = function (data) {
        Console.WriteLine("level 2 received data: " + data);
    };
    postMessage("[data from level 2 by postMessage]");
    parentPort.postMessage("[data from level 2 by parentPort.postMessage]");

    //setInterval(() => { Console.WriteLine("^level 2 is running: " + new Date())},1000);
})();
