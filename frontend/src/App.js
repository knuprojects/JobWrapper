import { Route, Routes } from 'react-router-dom'
import Login from './LogIn/Login'
import Registration from './Registration/Registration';
import Main from './MainPage/Main';
function App() {
  const isAuthenticated = localStorage.getItem('token') !== null;

  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/registration" element={<Registration />} />
        <Route path = "/login" element = {<Login />} />
        {isAuthenticated && <Route path='/main' element = {<Main/>}/>}
        {!isAuthenticated && <Route path="/login" element={<Login />} />}
      </Routes>
    </div>
  );
}

export default App;
