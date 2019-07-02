let currentSource;

let context;

let connection;

let currentAudioLink;

let baseURL = window.location.href.replace(/(Pages.*$|index.html.*$)/, '');

let langID;

let performanceID = sessionStorage.performanceId;

let languageId;

let pauseTime = 0;

let tempBuffer;

let pauseBuffer;

let isLoaded;

let timeDiff;

let dict = [];

let shouldReload = false;

let userAgent = window.navigator.userAgent;

const connectionButton = document.getElementById('connecting-button');

const linkPart = document.getElementById('link-part');

const streamingPart = document.getElementById('streaming-part');

window.onload = goToStreamingPart();

function goToStreamingPart() {
    'use strict';
    console.log(window.location.href)

    if (sessionStorage.languageId === undefined || sessionStorage.performanceId === undefined)
        window.location.href = baseURL + 'Pages/performances.html';

    langID = sessionStorage.languageId;

    languageId = '_' + langID;

    connectionButton.addEventListener('click', connectToStream);
}

function backToMain() {
    window.location.href = sessionStorage.baseURL;
}

function connectToStream() {
    'use strict';

    let AudioContext = window.AudioContext || window.webkitAudioContext;

    context = new AudioContext();

    createAndPlaySilent();

    connectToHub();

    changeButton();
}

function changeButton() {
    'use strict';

    connectionButton.style.background = '#e0d6fb';
    connectionButton.style.color = '#313f72';
    connectionButton.disabled = true;
    connectionButton.textContent = 'Connected';
}

function handleMessage(link, offset, paused) {
    'use strict';

    console.log('We get: ' + link);

    switch (link) {
        case 'Start':
            startStream(shouldReload);
            break;
        case 'End':
            endStream();
            break;
        case 'Resume':
            resumeStream();
            break;
        case 'Pause':
            pauseStream(offset);
            break;
        case currentAudioLink:
           restartCurrentAudio();
           break;
        default:
            playNewAudio(link, offset, paused);
            break;
    }
}

function startStream(shouldReload) {
    'use strict';
    if(shouldReload)
    {
        location.reload(true);
    }

    isLoaded = false;
    console.log(currentAudioLink);
    currentAudioLink = baseURL + 'audio/Waiting.mp3';

    saveAndPlayAudio(currentAudioLink, true);

    preLoadAudio();
}

function getAudios() {
    'use strict';
    //console.log('http://192.168.0.100:5000/api/Audio/preload/' + performanceID + '/' + langID);
    return fetch(baseURL + 'api/Audio/preload/' + performanceID + '/' + langID)
        .then(response => {
            if (!response.ok) {
                throw new Error('HTTP error, status = ' + response.status);
            }
            return response.json();
        });
}


function preLoadAudio() {
    'use strict';

    //console.log(performanceID);
    //console.log(langID);
    getAudios().then(response => {
        //console.log(response.filename);
        response.forEach(response => savePreLoadAudio(response.fileName));
    })
        .catch(error =>
            console.log(error)
        );
}

function savePreLoadAudio(URL) {
    //'use strict';
    console.log("from preload");
    link = baseURL + 'audio/' + URL;

    //console.log("from savePreloadAudio"+link);
    var name = link;
    return fetch(link)
        .then(response => response.arrayBuffer())
        .then(arrayBuffer => {
            //console.log("start download");
            context.decodeAudioData(
                arrayBuffer,
                audioBuffer => {
                    //console.log("start pushing");
                    dict.push({
                        key: name,
                        value: audioBuffer
                    });
                }
            )
}
        )
}

function endStream() {
    'use strict';

    currentSource.stop();

    window.location.href = baseURL + 'Pages/links.html';
    shouldReload = true;
    console.log(shouldReload);
}

function resumeStream() {
    'use strict';
    
    timeDiff = new Date().getTime();
    play(tempBuffer, false, pauseTime);
}

function pauseStream(offset) {
    'use strict';
    
    pauseTime = offset;
    timeDiff = new Date().getTime();
    console.log(pauseTime);

    tempBuffer = currentSource.buffer;
    stop();
}

function displayLinks() {
    'use strict';

    streamingPart.style.display = '';

    linkPart.style.display = 'flex';
}

function restartCurrentAudio(offset) {
    'use strict';

    stop();

    saveAndPlayAudio(currentAudioLink, false, offset);
}

function playNewAudio(link, offset, paused) {
    console.log("from playNewAudio" + link);
    link = baseURL + 'audio/' + link + languageId + '.mp3';

    currentAudioLink = link;
    saveAndPlayAudio(currentAudioLink, false, offset, paused);
}

function saveAndPlayAudio(URL, audioLoop, offset, paused) {
    'use strict';
    timeDiff = (new Date()).getTime();
    //console.log("URL " + URL);
    //console.log(dict);
    if (dict.some(e => e.key === URL)) {
        console.log("we are in");
        //console.log(dict.find((e) => e.key === URL).value);
        play(dict.find((e) => e.key === URL).value, audioLoop, offset);
    } else {
        console.log("not in");
        return fetch(URL)
            .then(response => response.arrayBuffer())
            .then(arrayBuffer =>
                context.decodeAudioData(
                    arrayBuffer,
                    audioBuffer => { 
                        play(audioBuffer, audioLoop, offset); 
                        if (paused === true) {
                            pauseStream(offset);
                        }
                        isLoaded = true;
                    },
                    error => console.error(error)
                )
            )
    }
}

function connectToHub() {
    'use strict';

    connection = new signalR.HubConnectionBuilder()
        .withUrl(baseURL + "StreamHub")
        .build();

    connection.serverTimeoutInMilliseconds = 600000;

    connection.on("ReceiveMessage", function (message, offset, paused) {
        handleMessage(message, offset, paused);
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });
}

function createAndPlaySilent() {
    'use strict';

    pauseBuffer = context.createBuffer(2, context.sampleRate * 3, context.sampleRate);

    for (let channel = 0; channel < pauseBuffer.numberOfChannels; channel++) {

        let nowBuffering = pauseBuffer.getChannelData(channel);

        for (let i = 0; i < pauseBuffer.length; i++) {
            // nowBuffering[i] = Math.random() * 2 - 1;
            nowBuffering[i] = 0;
        }
    }

    const pauseSource = context.createBufferSource();

    pauseSource.buffer = pauseBuffer;

    pauseSource.connect(context.destination);

    pauseSource.loop = true;

    pauseSource.start();
}

function play(currentBuffer, loopCondition, offset=0) {
    //'use strict';

    timeDiff = new Date().getTime() - timeDiff;

    if (currentAudioLink === baseURL + 'audio/Waiting.mp3')
            isLoaded = true;

    if (currentSource !== undefined) {
        if (!isLoaded) {
            isLoaded = true; 
            return;
        }
        stop();
    }

    currentSource = context.createBufferSource();

    currentSource.buffer = currentBuffer;

    currentSource.connect(context.destination);

    currentSource.loop = loopCondition;

    let offsetFinal = timeDiff / 1000 + offset;
    if (offsetFinal < 0)
        offsetFinal = 0;

    currentSource.start(0, offsetFinal);
}

function stop()
{
    if(/Android/.test(userAgent)) {
        currentSource.stop();
    }
    else {
        currentSource.buffer = null;
    }
}

function getData(api) {
    'use strict';
    return fetch(api)
        .then(response => {
            console.log(response);
            if (!response.ok) {
                throw new Error('HTTP error, status = ' + response.status);
            }
            return response.json();
        });
}