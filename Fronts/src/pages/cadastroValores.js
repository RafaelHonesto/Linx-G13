import React, { Component } from 'react'
import '../css/perfil.css';
import '../css/valores.css';
import '../css/despesas.css';
import BarraLateral from '../components/barra'
import EnfeiteTela from '../components/enfeiteTela';
import axios from 'axios';
import { parseJwt } from '../services/auth';
import Swal from 'sweetalert2';

class valores extends Component {

    constructor(props) {
        super(props);
        this.state = {
            listaTiposDespesa: [],
            listaValor: [],
            idTipoDespesa: '',
            nome: '',
            descricao: '',
            valor: '',
            data: new Date()
        }
    }

    selecionarEntrada = (event) => {

        event.preventDefault();

        const button = document.getElementById('button1')
        const button2 = document.getElementById('button2')
        button.classList.add('mudarCor')
        button2.classList.remove('mudarCor')
    }

    selecionarSaida = (event) => {

        event.preventDefault();

        const button = document.getElementById('button2')
        const button2 = document.getElementById('button1')
        button.classList.add('mudarCor')
        button2.classList.remove('mudarCor')
    }

    listarValores = () => {
        axios('http://localhost:5000/api/Valor', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => this.setState({ listaValor: response.data }))

            .catch(erro => console.log(erro))
    }

    cadastrarValores = (event) => {

        event.preventDefault();

        axios.post('http://localhost:5000/api/Valor', {
            idTipoDespesa: this.state.idTipoDespesa,
            nome: this.state.nome,
            descricao: this.state.descricao,
            dataDespesa: this.state.data
        }, {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => {
                if (response.status === 201) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Despesa cadastrada',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }

                this.setState({
                    nome: '', descricao: '', idTipoDespesa: '', valor: ''
                })

                this.listarDespesas();
            })

            .catch(erro => console.log(erro))
    }

    componentDidMount() {
        this.listarValores();
    }

    funcaoMudaState = (campo) => {
        this.setState({ [campo.target.name]: campo.target.value })
    }

    render() {
        return (
            <section className="bodyDespesa">
                <section className="corpoDespesas">
                    <form onSubmit={this.cadastrarDespesa} className="inputsValores">
                        <h4>Cadastro de valores</h4>
                        <div className='buttonTipoValores'>
                            <button onClick={this.selecionarEntrada} id='button1' className="buttonTipoValores2">Entrada</button>
                            <button className="buttonTipoValores3" id='button2' onClick={this.selecionarSaida}>Saída</button>
                        </div>
                        <input placeholder="Titulo" type="text" name='nome' value={this.state.nome} onChange={this.funcaoMudaState} />
                        <input placeholder="Valor" type="text" name='nome' value={this.state.nome} onChange={this.funcaoMudaState} />
                        <input placeholder="CNPJ" type="text" name='valor' value={this.state.valor} onChange={this.funcaoMudaState} />
                        <input placeholder="Descrição (opcional)" type="text" name='descricao' value={this.state.descricao} onChange={this.funcaoMudaState} />
                        <div className="contentComprovanteValores">
                            <h4>Comprovante</h4>
                            <input placeholder="Comprovante" type="file" id='file' name='descricao' value={this.state.descricao} onChange={this.funcaoMudaState} />
                            <label for="file" className="inputImagemValores">Escolha uma imagem</label>
                        </div>

                        <button type='submit' className="buttonValor">Adicionar +</button>
                    </form>

                    <div className="listaDespesas">
                        <table className="tabela-despesas">
                            <tr className='tituloDespesas'>
                                <th>Nome</th>
                                <th>Tipo</th>
                                <th>Data</th>
                                <th>Descrição</th>
                            </tr>

                            {
                                this.state.listaValor.map((dados) => {
                                    return (
                                        <tr className='trDespesa'>
                                            <td>{dados.descricao}</td>
                                            <td>{new Intl.DateTimeFormat('pt-BR').format(new Date(dados.dataValor))}</td>
                                            <td>{dados.valor}</td>
                                            <button disabled={parseJwt().Role === '3' ? true : false}>Editar</button>
                                        </tr>
                                    )
                                })
                            }



                        </table>
                    </div>

                </section>

                <BarraLateral />

                <EnfeiteTela />

            </section>
        );

    }
}

export default valores;
