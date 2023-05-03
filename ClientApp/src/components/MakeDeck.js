import React, { Component } from 'react';
import AsyncSelect from 'react-select/async';
import { Input } from 'reactstrap';

const options = [
    { value: 'red', label: 'Red' },
    { value: 'blue', label: 'Blue' },
    { value: 'green', label: 'Green' },
]

export class MakeDeck extends Component {
    static displayName = MakeDeck.name;

    constructor(props) {
        super(props);
        this.state = { deck: [], card1: '', card2: '' };

    }

    render() {
        const loadOptions1 = async (inputValue, callback) => {
            const response = await fetch(`/deck/autocomplete1?card1=${inputValue}`);
            const data = await response.json();
            console.log(data);
            callback(data.map(x => ({ value: x, label: x })));

        };

        const loadOptions2 = async (inputValue, callback) => {
            const response = await fetch(`/deck/autocomplete2?card1=${this.state.card1}&card2=${inputValue}`);
            const data = await response.json();
            console.log(data);
            callback(data.map(x => ({ value: x, label: x })));

        };

        return (
            <div>
                <h1 id="tableLabel">Find a Deck</h1>
                <p>Enter two cards and I'll give you a deck using both.</p>
                {/*<Input type="text" name="card1" id="card1" placeholder="Card 1" onChange={(e) => this.setState({ card1: e.target.value })} />*/}
                <AsyncSelect name="card1" id="card1" cacheOptions loadOptions={loadOptions1} onChange={(e) => this.setState({ card1: e.value })} />
                <br />
                {/*<Input type="text" name="card2" id="card2" placeholder="Card 2" onChange={(e) => this.setState({ card2: e.target.value })} />*/}
                <AsyncSelect name="card2" id="card2" cacheOptions loadOptions={loadOptions2} onChange={(e) => this.setState({ card2: e.value })} />
                <button className="btn btn-primary" onClick={this.populateDeck.bind(this)}>Find Deck</button>
                <table className="table table-striped" aria-labelledby="tableLabel">
                    <thead>
                        <tr>
                            <th>Count</th>
                            <th>Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.deck.map(card =>
                            <tr key={card.name}>
                                <td>{card.count}</td>
                                <td>{card.name}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
                {this.state.deck.length === 0 ? <p>Enter two cards that are in a deck together.</p> : <p></p>}
            </div>
        );
    }

    async populateDeck() {
        const response = await fetch(`/deck?card1=${this.state.card1}&card2=${this.state.card2}`);
        const data = await response.json();
        console.log(data);
        this.setState({ deck: data });
    }
}
