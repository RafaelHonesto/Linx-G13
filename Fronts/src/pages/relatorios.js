import { Component } from "react";
import '../css/relatorio.css'
import BarraLateral from '../components/barra'
import axios from "axios";

class relatorios extends Component {

    constructor(props) {
        super(props);
        this.state = {
            nomeSetor: '',
            nomeSetores: '',
            listaSetores: []
        }
    }

    buscarSetor2 = async () => {

        await axios(`http://localhost:5000/api/Setores/buscar/${this.state.nomeSetores}`, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(resposta => {
                if (resposta.status === 200) {
                    this.setState({
                        nomeSetores: resposta.data.nome
                    })
                }
            })

            .catch(erro => console.log(erro))

        console.log(this.state.nomeSetor)
    }

    listarSetores = () => {
        axios(`http://localhost:5000/api/Setores`, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(resposta => this.setState({ listaSetores: resposta.data }))

            .catch(erro => console.log(erro))
    }

    buscarSetor = async () => {

        await axios(`http://localhost:5000/api/Setores/buscar/${this.state.nomeSetor}`, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(resposta => {
                if (resposta.status === 200) {
                    this.setState({
                        idSetor: resposta.data.idSetor
                    })
                }

                if (this.state.idSetor !== "") {
                    this.cadastrarUsuario();
                }
            })

            .catch(erro => console.log(erro))

        console.log(this.state.nomeSetor)
    }

    componentDidMount(){
        this.listarSetores();
    }


    render() {
        return (
            <section className='body'>

                <section className="corpoRelatorio">
                    <div>
                        <select className="selectRelatorio">
                        <option>Selecione um setor</option>
                            {
                                this.state.listaSetores.map((dados) => {
                                    return (
                                        <option value={dados.idSetor}>{dados.nome}</option>
                                    )
                                })
                            }
                        </select>

                        <table className="tabelaRelatorio">
                            <div className="tituloTabelaRelatorio">
                                <h4>Relatório</h4>
                                <div className='subtitulosTabelaRelatorio'>
                                    <h5>Data</h5>
                                    <h5>Valor</h5>
                                </div>
                            </div>

                            <div className="linhaTabelaRelatorio">
                                <h2>Conta de luz</h2>
                                <div className='subLinhaTabelaRelatorio'>
                                    <h5>30/06/2003</h5>
                                    <h5>R$200,00</h5>
                                </div>
                            </div>
                            <div className="linhaTabelaRelatorio">
                                <h2>Conta de luzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz</h2>
                                <div className='subLinhaTabelaRelatorio'>
                                    <h5>30/06/2003</h5>
                                    <h5>R$200,00</h5>
                                </div>
                            </div>
                        </table>

                        <button className='buttonDownRelatorio'>Fazer dowload do relatório</button>
                    </div>
                </section>

                <BarraLateral />


            </section>
        )
    }
}

export default relatorios