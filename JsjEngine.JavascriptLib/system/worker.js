import { MessageChannel, isTransferable } from './channel';
import { isMainThread, WorkerImpl, PostJson } from '@jsj/internal/worker';

const { port2 } = new MessageChannel();
function postMessageInternal(hostPostJsonFunc, data, transferList) {
    hostPostJsonFunc(JSON.stringify(data));
    //todo: check transferList
}
export function Worker(workLoad) {
    const childImpl = WorkerImpl.CreateChild(this);
    this.postMessage = function (message, transferList) {
        postMessageInternal(childImpl.PostJson, message, transferList);
    }
    this.terminate = childImpl.Dispose;
    if (typeof (workLoad) === 'string') {//todo:should check workLoad is a valid url
        childImpl.PostExecuteScriptFile(workLoad);
    } else {//todo: case to pass script code to worker
        childImpl.PostExecuteScriptCode(workLoad);
    }
};
export function postMessage(message, transferList) {
    if (!isMainThread) {
        postMessageInternal(PostJson, message, transferList);
    }
}
export let parentPort = port2;
export { isMainThread }

