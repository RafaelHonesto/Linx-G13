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

    botaoBaixar() {
        const linhaTabela = document.querySelectorAll('tr')
        const botaoBaixar = document.getElementById('botaoBaixar')

        botaoBaixar.addEventListener('click', () => {



            const csv = Array.from(linhaTabela)
                .map(row => Array.from(row.cells)
                    .map(cell => cell.textContent)
                    .join(',')
                )
                .join('\n')

            console.log(csv)

            botaoBaixar.setAttribute(
                'href',
                `data:text/csv;charset=utf-8,${encodeURIComponent(csv)}`
            )

            botaoBaixar.setAttribute(
                'download',
                'Relatório.csv'
            )

        })
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

    componentDidMount() {
        this.listarSetores();
        this.botaoBaixar();
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
                            <thead >
                                <tr className="tituloTabelaRelatorio">
                                    <th>Relatório</th>
                                    <th>Data</th>
                                    <th>Valor</th>
                                </tr>

                            </thead>

                            <tbody>
                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Água</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$200</td>
                                </tr>

                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Energia</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$450</td>
                                </tr>

                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Cadeira</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$100</td>
                                </tr>

                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Venda curso</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$100</td>
                                </tr>
                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Água</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$200</td>
                                </tr>

                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Energia</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$450</td>
                                </tr>

                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Cadeira</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$100</td>
                                </tr>

                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Venda curso</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$100</td>
                                </tr>
                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Água</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$200</td>
                                </tr>

                                <tr className="linhaTabelaRelatorio">
                                    <td id='trTabela'>Energia</td>
                                    <td id='trTabela'>30/06/2003</td>
                                    <td id='trTabela'>R$450</td>
                                </tr>

                            </tbody>


                        </table>

                        <div className='buttonDownRelatorio'>
                            <a role='button' id='botaoBaixar'>Fazer dowload do relatório</a>
                        </div>

                    </div>
                </section>

                <BarraLateral />


            </section>
        )
    }
}

export default relatorios