declare module '@jsj/internal/worker' {
    /**
     * True if this code is not running inside of a Worker thread.
     * https://nodejs.org/api/worker_threads.html#workerismainthread
     * */
    export const isMainThread: boolean;

    /**
    * Create a 'PostJsonEvent' and add to the event queue of 'parent context'.
    * This method will be null or undefined if isMainThread = true as there is no 'parent context' for main thread
    * @param json
    * @param sourceWorker
    */
    export function PostJson(json: string, sourceWorker?: JsjEngine.ScriptObject)


    export const WorkerImpl: JsjEngine.JsjRuntime;
}