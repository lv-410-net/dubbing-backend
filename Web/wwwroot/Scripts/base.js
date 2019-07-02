window.onload = () => {
    sessionStorage.baseURL = window.location.href.replace(/(Pages.*$|index.html.*$)/, '');
}

function onClick() {
    window.location.href = sessionStorage.baseURL + 'Pages/performances.html';
}