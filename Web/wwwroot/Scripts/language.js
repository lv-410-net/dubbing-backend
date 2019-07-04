let baseURL = window.location.href.replace(/(Pages.*$|index.html.*$)/, '');

let languagesAPI;

let languagePart = document.getElementById('language-selection-part');

window.onload = goToLanguagesPart;

function goToLanguagesPart() {
    'use strict';

    if (sessionStorage.performanceURL === undefined)
        window.location.href = baseURL + 'Pages/performances.html';

    languagesAPI = sessionStorage.performanceURL + '/languages';

    getData(languagesAPI).then(response => {
        response.forEach(language => {
            let button = document.createElement('button');
            button.setAttribute('class', "languagesButton");
            let name = document.createTextNode(language.name);
            button.addEventListener('click', () => {
                sessionStorage.languageId = language.id;
                window.location.href = baseURL + 'Pages/stream.html';
            }
            );
            button.appendChild(name);
            languagePart.appendChild(button);
        })
    }).catch(error =>
        console.log(error)
    );
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