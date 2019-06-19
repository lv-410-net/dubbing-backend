let currentSource;

let context;

let connection;

let currentAudioLink;

let languageId;

let pauseTime = 0;

let tempBuffer;

let isLoaded;

const performancesAPI = 'api/performance';

let languagesAPI;

const connectionButton = document.getElementById('connecting-button');

const performancePart = document.getElementById('performance-selection-part');

const languagePart = document.getElementById('language-selection-part');

const linkPart = document.getElementById('link-part');

const streamingPart = document.getElementById('streaming-part');


window.onload = init;


function init() {
    'use strict';

    getData(performancesAPI).then(response => {
        response.forEach(performance => {
            let button = document.createElement('button');
            button.setAttribute('class', "performanceButton");
            let title = document.createTextNode(performance.title);
            button.addEventListener('click', () =>
                goToLanguagesPart(performance.id)
            );
            button.appendChild(title);
            performancePart.appendChild(button);
        })
    }).catch(error =>
        console.log(error)
    );

    performancePart.style.display = 'flex';
}

function goToLanguagesPart(performanceId) {
    'use strict';
    
    languagesAPI = 'api/performance/' + performanceId +'/languages/';
    
    getData(languagesAPI).then(response => {
        response.forEach(language => {
            let button = document.createElement('button');
            button.setAttribute('class', "languagesButton");
            let name = document.createTextNode(language.name);
            button.addEventListener('click', () =>
                goToStreamingPart(language.id)
            );
            button.appendChild(name);
            languagePart.appendChild(button);
        })
    }).catch(error =>
        console.log(error)
    );

    performancePart.style.display = 'none';
    languagePart.style.display = 'flex';
}

function goToStreamingPart(langId) {
    'use strict';

    languageId = '_' + langId;
    
    languagePart.style.display = 'none';

    streamingPart.style.display = 'flex';

    connectionButton.addEventListener('click', connectToStream);
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
    
    connectionButton.style.backgroundColor = 'green';
    connectionButton.disabled = true;
    connectionButton.textContent = 'You are connected to stream';
}

function handleMessage(link, time) {
    'use strict';

    console.log('We get: ' + link);

    switch (link) {
        case 'Start':
            startStream();
            break;
        case 'End':
            endStream();
            break;
        case 'Resume':
            resumeStream();
            break;
        case 'Pause':
            pauseTime = time;
            console.log(time);
            pauseStream();
            break;
        case currentAudioLink:
            restartCurrentAudio();
            break;
        default:
            playNewAudio(link, time);
            break;
    }
}

function startStream() {
    'use strict';

    isLoaded = false;
    console.log(currentAudioLink);
    currentAudioLink = 'audio/Waiting.mp3';

    saveAndPlayAudio(currentAudioLink, true);

    isLoaded = true;
}

function endStream() {
    'use strict';

    currentSource.stop();

    displayLinks();
}

function resumeStream() {
    'use strict';

    play(tempBuffer, false, pauseTime);
}

function pauseStream() {
    'use strict';

    tempBuffer = currentSource.buffer;
    currentSource.stop();
}

function displayLinks() {
    'use strict';

    streamingPart.style.display = '';

    linkPart.style.display = 'flex';
}

function restartCurrentAudio() {
    'use strict';

    currentSource.stop();

    saveAndPlayAudio(currentAudioLink);
}

function playNewAudio(link, time) {
    'use strict';

    link = 'audio/' + link + languageId + '.mp3';

    if (currentSource !== undefined) {
        currentSource.stop();
    }
    currentAudioLink = link;

    saveAndPlayAudio(currentAudioLink, false, time);
}

function saveAndPlayAudio(URL, audioLoop, time = 0) {
    //'use strict';

    console.log(URL);

    return fetch(URL)
        .then(response => response.arrayBuffer())
        .then(arrayBuffer =>
            context.decodeAudioData(
                arrayBuffer,
                audioBuffer => play(audioBuffer, audioLoop, time),
                error => console.error(error)
            )
        )
}

function connectToHub() {
    'use strict';

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/StreamHub")
        .build();

    connection.on("ReceiveMessage", function (message, time) {
        handleMessage(message, time);
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });
}

function createAndPlaySilent() {
    'use strict';

    const pauseBuffer = context.createBuffer(2, context.sampleRate * 3, context.sampleRate);

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

function play(currentBuffer, loopCondition, time) {
    'use strict';

    currentSource = context.createBufferSource();

    currentSource.buffer = currentBuffer;

    currentSource.connect(context.destination);

    currentSource.loop = loopCondition;

    currentSource.start(0, time);
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