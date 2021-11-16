import { Component } from "react";
import { Link } from "react-router-dom";
import '../css/barraLateral.css'
import { parseJwt } from "../services/auth";
import Swal from "sweetalert2";

// let items = document.getElementsByClassName('li');

// items.forEach(item => item.classList.remove('active'))

// items.forEach(item => { item.addEventListener('click', () => { item.classList.add('active') }) })

class BarraLateral extends Component {

    constructor(props){
        super(props);
        this.state = {
            logout : false
        }
    }

    certezaSair () {

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
          }).then((result) => {
            if (result.isConfirmed) {
              this.logout()
            }
          })
    }

    logout(){
        

            //   localStorage.removeItem('token-login')

            console.log(this.state.logout)
    }



    render() {
        return (
            <div class="container">
                <ul>
                    <Link to='/home'><li className="li" >Início</li></Link>
                    <Link to='/valores'><li className="li" hidden={parseJwt().Role === '1' ? true : false}>Valores</li></Link>
                    <Link to='/despesas'><li className="li" hidden={parseJwt().Role === '1' ? true : false}>Despesas</li></Link>
                    <Link to='/home'><li className="li" hidden={parseJwt().Role === '1' ? true : false}>Dashboard</li></Link>
                    <Link to='/perfil'><li className="li" hidden={parseJwt().Role === '1' ? true : false}>Perfil</li></Link>
                    <li onClick={this.certezaSair} className="li">Sair</li>
                </ul>
            </div>
        )
    }
}

export default BarraLateral