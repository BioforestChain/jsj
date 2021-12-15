import { setTimeout, setInterval, clearInterval, clearTimeout } from "system/timer"

(function () {
    let output = "this is the output from timer Demo..."

    Console.WriteLine("time before settimeout: " + new Date());

    let timerId = setTimeout(() => {
        Console.WriteLine("first setTimeout 3 seconds later: " + new Date());
    }, 3000)

    setTimeout(() => {
        Console.WriteLine("second setTimeout 2 seconds later: " + new Date());
    }, 2000)

    setTimeout(() => {
        Console.WriteLine("third setTimeout 1 seconds later: " + new Date());
    }, 1000)

    Console.WriteLine("time after settimeout: " + new Date());

    Promise.resolve().then(() => {
        Console.WriteLine("Promise done ");
    });

    let intervalRunTime = 0;
    let intervalId = setInterval(() => {
        Console.WriteLine("this message is shown every 3 seconds: " + new Date());
        Console.WriteLine("setInterval run time: " + intervalRunTime++);
        if (intervalRunTime > 10) {
            Console.WriteLine("after run 10 times, now setInterval will be cleared ");
            Console.WriteLine("&&&&&&&&&&&&&&&&intervalId: " + intervalId);
            clearInterval(intervalId);
        }
    }, 3000);

    setInterval(() => {

        Console.WriteLine("keeping alive: " + new Date());
    }, 5000);

    //let timerId2 = setInterval(() => {
    //    Console.WriteLine("setInterval runs once per second: " + new Date());
    //    Console.WriteLine("timer id: " + timerId2);
    //}, 1000)

    return output;
})();