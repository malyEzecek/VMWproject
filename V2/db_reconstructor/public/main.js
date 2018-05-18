(function() {
    let socket;

    let timeEl, countEl, wordEl, buttonsContainer, runButton, stopButton, dataButton, reportButton;

    function init() {
        socket = io.connect("http://localhost:3001")
        timeEl = document.getElementById("time");
        countEl = document.getElementById("count");
        wordEl = document.getElementById("current-word");
        buttonsContainer = document.getElementById("button-container");
        runButton = document.getElementById("run-button");
        stopButton = document.getElementById("stop-button");
        dataButton = document.getElementById("download-data");
        reportButton = document.getElementById("download-report");
        reset();

        runButton.onclick = () => run(true);
        stopButton.onclick = () => run(false);

        reportButton.onclick = () => downloadFile("/results.csv");
        dataButton.onclick = () => downloadFile("/data.json");

        socket.on("record", data => {
            let timestamp = data.timestamp.toFixed(0);
            let mins = (timestamp / 60).toFixed(0);
            if (mins < 10)
                mins = "0" + mins;
            let secs = (timestamp % 60);
            if (secs < 10)
                secs = "0" + secs;

            timeEl.value =  mins + ":" + secs;
            countEl.value = data.size;
            wordEl.value = data.query;
        });

        socket.on("done", () => {
            run(false);
        })
    }

    function reset() {
        timeEl.value = "00:00";
        wordEl.value = "";
        countEl.value = 0;
    }

    function run(value) {
        if (value)
            reset();

        runButton.disabled = value;
        stopButton.disabled = !value;
        buttonsContainer.hidden = value;
        socket.emit("run", value);
    }

    function downloadFile(sUrl) {
        //If in Chrome or Safari - download via virtual link click
        if (downloadFile.isChrome || downloadFile.isSafari) {
            //Creating new link node.
            var link = document.createElement('a');
            link.href = sUrl;
    
            if (link.download !== undefined){
                //Set HTML5 download attribute. This will prevent file from opening if supported.
                var fileName = sUrl.substring(sUrl.lastIndexOf('/') + 1, sUrl.length);
                link.download = fileName;
            }
    
            //Dispatching click event.
            if (document.createEvent) {
                var e = document.createEvent('MouseEvents');
                e.initEvent('click' ,true ,true);
                link.dispatchEvent(e);
                return true;
            }
        }
    
        // Force file download (whether supported by server).
        var query = '?download';
    
        window.open(sUrl + query);
    }
 
    downloadFile.isChrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
    downloadFile.isSafari = navigator.userAgent.toLowerCase().indexOf('safari') > -1;

    document.onreadystatechange = function() {
        if (document.readyState === "complete")
            init();
    };
})()