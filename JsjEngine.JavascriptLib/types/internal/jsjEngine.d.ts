declare namespace JsjEngine {
    /**
     * Microsoft.ClearScript.ScriptObject
     * */
    class ScriptObject {}
    /**
     * The jsj javascript engine
     * */
    class JsjRuntime {

        /**
         * Add script code to the event queue so that runtime can execute it in next 'loop'.
         * This method is designed to be used in the host side as an entry point of a new initiated runtime instance.
         * @param code the script code to execute
         */
        PostExecuteScriptCode(code: string)

        /**
         * Add a script file to the event queue so that runtime can execute it in next 'loop'.
         * The runtime will first download the code from the location where 'fileUrl' pointed to before starting to excute them.
         * This method is designed to be used both in the host side (as an entry point of a new initiated runtime instance)
         * and in the script side (to pass workload to a new created worker)
         * @param fileUrl
         */
        PostExecuteScriptFile(fileUrl:string)


        /**
         * Create a child JsjRuntime instance.
         * @param worker The 'script side' of jsjRuntime instance, i.e. a Worker instance
         */
        CreateChild(worker: ScriptObject): JsjRuntime

        /**
         * Create a 'PostJsonEvent' and add to the event queue.
         * @param json
         * @param sourceWorker
         */
        PostJson(json: string, sourceWorker?: ScriptObject)

        /**
         Add an 'exit' event to the event queue and wait for current loop to finish,
         then dispose all the timers, finally dispose v8 script engine itself.
         * */
        Dispose()
    }
}