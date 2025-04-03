import { Router, Route, A } from "@solidjs/router";
import { render } from 'solid-js/web';
import './App.css'
import Table from './table/Table';
import Home from ".";

const App = (props: any) => (
    <>
      <h1>Woho</h1>
      <A href="/"> Home </A>
      <A href="/table"> Table </A>
      {props.children}
    </>
); 
const root = document.getElementById("root")
render(() => (
  <Router root={App}>
    <Route path="/" component={Home}/>
    <Route path="/table" component={Table} />
  </Router>
  ), 
  root!
);