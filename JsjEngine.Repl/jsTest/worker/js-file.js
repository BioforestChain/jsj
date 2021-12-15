import { Worker } from "system/worker"
import { setTimeout, setInterval, clearInterval, clearTimeout } from "system/timer"
(function () {
    Console.WriteLine("app started: " + new Date());
    let workerLevel1 = new Worker("D:/projects/BFS/JsjEngine/JsjEngine.Repl/jsTest/worker/worker-level1.js");
 

    workerLevel1.postMessage("[data from main]");
    workerLevel1.onmessage = function (data) {
        Console.WriteLine("main js received data: " + data );
    }

/*    setTimeout(() => {
        workerLevel1.terminate();
        Console.WriteLine("------------after 30 seconds running, now workerLevel1 is terminated : " + new Date());
    }, 30000)

    setInterval(() => { Console.WriteLine('main is running ' + new Date()); }, 3000);*/

})();