import React, {Component} from 'react'

export class Prevision extends Component {
    constructor(props) {
        super(props);
        this.state = {imageAsString: undefined, loading: true};
    }

    componentDidMount() {
        this.getImage()
            .then(r => console.log('Immagine OK'))
            .catch(err => console.error('Errore -> ' + err.message));
    }

    render() {
        const contents = this.state.loading ? (
            <p>
                <em>Loading...</em>
            </p>
        ) : (
            <img src={`data:image/jpeg;base64,${this.state.imageAsString}`}  alt='Prevision'/>
        );

        return (
            <div>
                <h1>Prevision</h1>
                {contents}
            </div>
        )
    }
    
    async getImage() {
        const response = await fetch('api/prevision');
        const data = await response.json();
        
        this.setState({imageAsString: data, loading: false})
    }

}
