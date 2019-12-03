import React, {Component} from 'react'

export class FetchData extends Component {
    constructor(props) {
        super(props)
        this.state = {forecasts: [], customers: [], idCustomer: '', loading: true}

        this.handleIdCustomerChange = this.handleIdCustomerChange.bind(this)
    }

    componentDidMount() {
        this.populateWeatherData()
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby='tabelLabel'>
                <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
                </thead>
                <tbody>
                {forecasts.map(forecast => (
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        )
    }

    render() {
        const contents = this.state.loading ? (
            <p>
                <em>Loading...</em>
            </p>
        ) : (
            FetchData.renderForecastsTable(this.state.forecasts)
        )

        return (
            <div>
                <div className='form'>
                    <label>
                        ID Customer
                        <input
                            type='text'
                            value={this.state.idCustomer}
                            onChange={this.handleIdCustomerChange}
                        />
                    </label>

                    <input type='button' value='GET' onClick={() => this.handleGetOrders()}/>
                    <input type='button' value='CREATE' onClick={() => this.handleCreateOrder()}/>
                    <input type='button' value='DELETE' onClick={() => this.handleDeleteOrder()}/>
                    <input type='button' value='UPDATE' onClick={() => this.handleUpdateOrder()}/>
                </div>
                {this.customersAsList()}

                <h1 id='tabelLabel'>Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        )
    }

    handleIdCustomerChange(event) {
        this.setState({idCustomer: event.target.value})
    }

    customersAsList() {
        return this.state.customers.map(customer => (
            <p key={customer}>{customer}</p>
        ))
    }

    async populateWeatherData() {
        const response = await fetch('weatherforecast')
        const data = await response.json()
        this.setState({forecasts: data, loading: false})
    }

    async handleGetOrders() {
        const response = await fetch('orders/' + this.state.idCustomer)
        const data = await response.json()
        this.setState({customers: data, loading: false})
    }

    async handleCreateOrder() {
        await fetch('orders/insert')
    }

    async handleUpdateOrder() {
        await fetch('orders/update/' + this.state.idCustomer)
    }

    async handleGetOrder() {
        await fetch('orders/order/' + this.state.idCustomer)
    }

    async handleDeleteOrder() {
        await fetch('orders/delete/' + this.state.idCustomer)
    }
}
