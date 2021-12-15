# Description
jsjEngine is a javascript runtime based on [ClearScript](https://github.com/microsoft/ClearScript). The goal of this project is to provide a managed javascript runtime that allows various BFS libraries written in javascript to run without dependence of any webview.


# Implemented Features

* Timer related api(setTimeout, setInterval, clearInterval, clearTimeout)
* Web worker api(partial implemented)
* MessageChannel api(partial implemented)
* BFS custom module api(partial implemented)

# Planed Features
* jsj Module loader as discussed with @Zhaofeng
```
/*
 *
// system/exec.ts
import { argv } from '@jsj/internal/cmd';
import { fetch } from '@jsj/internal/net';
import { readFile } from '@jsj/internal/vfs';
const jsFile = argv.find(arg=>arg.endWiths('.js'));
if(jsFile){
  jsj.execModule(bfscode, const bfsResolver = (spe)=>{
     if(spe.sstartsWith( '@jsj/internal')){
        throw "PERM ERR"
     }
     if(spe === '@bfs/worker'){
       return readSnapFile('./worker.cachedata', 'file://bfs.zip')
     }
     if(spe === '@bfs/channel'){
       return bfs.channel
     }
     if(spe.startsWith('https://')){
       return bfs.exec(await fetch(spe), bfsResolver);
     }
     if(spe.startsWith('./')){
       readFile(spe)
     }else{
        readFile('./node_modules/'+spe)
     }
 })
}

  
 // systems/bundle.ts
 import * as worker from './worker'
 import * as channel from './channel';

const bfs = {
    worker,
    channel,
    dns
}

// bundle.js
import { MessageChannelImpl, IsTransferable } from '@jsj/internal/channel';

function MessagePort(portImpl: JsjEngine.MessagePort) {
}
 function MessageChannel() {};
 function isTransferable(obj):boolean {}

 let parentPort = port2;
 import { isMainThread } from '@jsj/internal/worker';
 const { port2 } = new MessageChannel();
 let parentPort$1 = port2;
 const worker = {
   parentPort: parentPort$1,
   isMainThread
 }

const bfs = {
    worker,
    channel
}


/// esbuild / rollup
// excludes: ["@jsj/internal/*"]
 */

// jsj bfs.zip
// bfs index.js --vfs=vfs.zip
// zip  src/ => vfs.zip

// code = fs.readFile(index.js)
/*
 bfs.exec(code, (spe)=>{

     if(spe .sstartsWith( '@jsj/internal')){
     throw "PERM ERR"
     }
     if(spe === '@bfs/worker'){
       return bfs.worker
     }
 })
 
 */
 ```
 * [SharedArrayBuffer](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SharedArrayBuffer) api
 * A .Net unit test project for jsjEngine implementation.
 * A javascript unit test project for jsjEngine default lib.
 * A .Net console app for jsjEngine REPL tool.
 