export const parseJwt = () => {

    let base64 = localStorage.getItem('token-login').split('.')[1];
    return JSON.parse(window.atob(base64));
    
}