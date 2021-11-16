import React, { Component } from 'react'
import '../css/perfil.css';
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

    listarValores = () => {
        axios('http://localhost:5000/api/Valor', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => this.setState({ listaValor: response.data }))

            .catch(erro => console.log(erro))
    }

    cadastrarDespesa = (event) => {

        event.preventDefault();

        axios.post('http://localhost:5000/api/Despesa', {
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
                    <form onSubmit={this.cadastrarDespesa} className="inputsDespesas">
                        <h4>Cadastro de despesas</h4>
                        <select name='idTipoDespesa' value={this.state.idTipoDespesa} onChange={this.funcaoMudaState} >
                            <option>Selecione o tipo da despesa</option>
                            <option value='1'>Energia</option>
                            <option value='2'>Água</option>
                            <option value='3'>Salário de funcionário</option>
                        </select>
                        <input placeholder="Nome" type="text" name='nome' value={this.state.nome} onChange={this.funcaoMudaState} />
                        <input placeholder="Valor" type="text" name='valor' value={this.state.valor} onChange={this.funcaoMudaState} />
                        <input placeholder="Descrição (opcional)" type="text" name='descricao' value={this.state.descricao} onChange={this.funcaoMudaState} />

                        <button type='submit'>Cadastrar</button>
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
