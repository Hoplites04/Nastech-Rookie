import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom'
import './App.css'
import DashBoard from './admin/pages/DashBoard.tsx'
import PhoneAdd from './admin/pages/PhoneAdd.tsx'
import PhoneList from './admin/pages/PhoneList.tsx'
import PhoneEdit from './admin/pages/PhoneEdit.tsx'
import PrivateRoute from './auth/PrivateRoute.tsx'
import Callback from './admin/pages/Callback';


function App() {

  return (
    <Router>
      <Routes>
        {/* Default route */}
        <Route path="/" element={<Navigate to="/admin/dashboard" replace />} />

        {/* Public dashboard */}
        <Route path="/admin/dashboard" element={<DashBoard />} />

        {/* Auth callback */}
        <Route path="/signin-oidc" element={<Callback />} />


        {/* Protected Routes */}
        <Route
          path="/admin/phoneadd"
          element={
            <PrivateRoute>
              <PhoneAdd />
            </PrivateRoute>
          }
        />
        <Route
          path="/admin/phoneedit/:id"
          element={
            <PrivateRoute>
              <PhoneEdit />
            </PrivateRoute>
          }
        />
        <Route
          path="/admin/phonelist"
          element={
            <PrivateRoute>
              <PhoneList />
            </PrivateRoute>
          }
        />
      </Routes>
    </Router>
  )
}

export default App
