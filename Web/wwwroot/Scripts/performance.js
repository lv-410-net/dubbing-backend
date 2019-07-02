let performancesAPI = sessionStorage.baseURL + 'api/performance';

let performancePart = document.getElementById('performance-selection-part');

window.onload = init;

function init() {
    'use strict';

    getData(performancesAPI).then(response => {
        response.forEach(performance => {
            let button = document.createElement('button');
            button.setAttribute('class', "performanceButton");
            let title = document.createTextNode(performance.title);
            button.addEventListener('click', () => {
                sessionStorage.performanceId = performance.id;
                sessionStorage.performanceURL = `${performancesAPI}/${performance.id}/`;
                window.location.href = sessionStorage.baseURL + 'Pages/languages.html';
            }
            );
            button.appendChild(title);
            performancePart.appendChild(button);
        })
    }).catch(error =>
        console.log(error)
    );

    //performancePart.style.display = 'flex';
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

function backToMain() {
    window.location.href = sessionStorage.baseURL;
}