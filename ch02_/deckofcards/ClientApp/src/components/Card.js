import React, { Component } from 'react';

export class Counter extends Component {
  static displayName = Counter.name;

  constructor(props) {
    super(props);
    this.state = { currentCount: 0 };
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  incrementCounter() {
    this.setState({
      currentCount: this.state.currentCount + 1
    });
  }

  render() {
    return (
      <div>
        <h1>Card</h1>
        <p aria-live="polite">
            <a src="" className="figure-img" />
        </p>

        <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
      </div>
    );
  }
}