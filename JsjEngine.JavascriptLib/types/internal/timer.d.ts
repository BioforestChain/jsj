declare module '@jsj/internal/timer' {
    /**
     * Create an 'InvokeCallbackEvent' and add to the event queue.
     * @param callback A function to be executed after the timer expires.
     * @param delay The time, in milliseconds that the timer should wait before the specified function is executed.
     * @param repeated Whether the callback will be invoked repeatedly (i.e. setInterval) .
     * @returns A positive integer value which can be passed to clearTimeout() to cancel the timeout.
     */
    export function InsertTimerCallback(callback: JsjEngine.ScriptObject, delay: number, repeated: boolean): number;

    /**
     * Remove an 'InvokeCallbackEvent' from the event queue.
     * @param timeoutId The identifier of the timeout you want to cancel. This ID was returned by the corresponding call to InsertTimerCallback().
     */
    export function RemoveTimerCallback(timeoutId :number)

}