import { Route, Routes } from 'react-router-dom'
import Login from './LogIn/Login'
import Registration from './Registration/Registration';
import Main from './MainPage/Main';
import PrivateRoute from './authContext';

function App() {
  const isAuthenticated = localStorage.getItem('token') !== null;

  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/registration" exact element={<Registration />} />
        {isAuthenticated && <Route path='/main' exact element = {<PrivateRoute component = {Main} />} />}
        {!isAuthenticated  && <Route path="/main" element={<Login />} />} 
      </Routes>
    </div>
  );
}

export default App;
