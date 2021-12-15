declare namespace JsjEngine{
    /**
     * MessageChannel implementation
     * https://developer.mozilla.org/en-US/docs/Web/API/MessageChannel
     */
    class MessageChannel {
        constructor(
            runtime: JsjRuntime
        );
        Port1: MessagePort;
        Port2: MessagePort;
    }
    /**
     * MessagePort implementation
     * https://developer.mozilla.org/en-US/docs/Web/API/MessagePort
     */
    class MessagePort {
        constructor(
            runtime: JsjRuntime
        )

        /**
         * Send json data from the port
         * Please note that this method assumes the data being sending is a valid json string
         * @param json The json data you want to send through the channel.
         */
        public PostJson(json: string);
    }
}

declare module '@jsj/internal/channel' {
    /**
     * The internal MessageChannel implementation
     * */
    export const MessageChannelImpl: JsjEngine.MessageChannel;

    /**
     * Check if an object is transferable.
     * For the v8 native types(e.g. ArrayBuffer), please check them with 'instanceof' before call this method,
     * as it will cross the 'js/c# boundary which is a potential performance hit.
     * @param obj object to check
     */
    export function IsTransferable(obj:object) : boolean;
}