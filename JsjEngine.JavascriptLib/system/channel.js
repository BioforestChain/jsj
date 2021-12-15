import { MessageChannelImpl, IsTransferable } from '@jsj/internal/channel';

function MessagePort(portImpl) {
    this.postMessage = function (data) {
        if (isTransferable(data)) {

        }
        else {
            portImpl.PostJson(JSON.stringify(data))
        }
    }
}
export function MessageChannel() {
    this.port1 = new MessagePort(MessageChannelImpl.Port1);
    this.port2 = new MessagePort(MessageChannelImpl.Port2);
};
export function isTransferable(obj) {
    //if (
    //    obj instanceof ArrayBuffer
    //    || obj instanceof MessagePort
    //    || obj instanceof ReadableStream
    //    || obj instanceof WritableStream
    //    || obj instanceof TransformStream
    //    || obj instanceof ImageBitmap
    //    || obj instanceof OffscreenCanvas
    //) {
    //    return true;
    //}

    return false;
}