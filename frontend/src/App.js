import './App.css';
import { Route, Routes } from 'react-router-dom'


import Login from './LogIn/Login'
import Registration from './Registration/Registration';
function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/registration" exact
          element={<Registration />}>
        </Route>
      </Routes>
    </div>
  );
}

export default App;
