import { MessageChannel } from 'system/channel';
import { Worker, postMessage, parentPort } from "system/worker"
import { setTimeout, setInterval, clearInterval, clearTimeout } from "system/timer"
(function () {

    var channel = new MessageChannel();

    parentPort.onmessage = function (data) {
        Console.WriteLine('level 1 received data from main: ' + data);
    };
    


    let workerLevel2 = new Worker("D:/projects/BFS/JsjEngine/JsjEngine.Repl/jsTest/worker/worker-level2.js");
    workerLevel2.onmessage = function (data) {
        Console.WriteLine('level 1 received data from workerLevel2: ' + data);
    };
    workerLevel2.postMessage("[data from level 1 sent to level 2]");


    postMessage("[data from level 1 sent to main by postMessage]");
    parentPort.postMessage("[data from level 1 sent to main by parentPort.postMessage]");

/*    setTimeout(() => {
        workerLevel2.terminate();
        Console.WriteLine("------------after 15 seconds running, now workerLevel2 is terminated : " + new Date());
    }, 15000)

    let counter = 0;
    setInterval(() => { Console.WriteLine('**level 1 is running ' + counter++); },2000);*/
 

})();