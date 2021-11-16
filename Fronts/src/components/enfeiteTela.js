import { Component } from "react";
import '../css/enfeiteTela.css'
import enfeiteTelas from '../img/efeiteTelas.png'

class enfeiteTela extends Component {

    render() {
        return (
            <section>
               <img src={enfeiteTelas} alt = 'imagem de enfeite da tela' className="imagemEnfeiteTela" />
            </section>
        )
    }
}

export default enfeiteTela