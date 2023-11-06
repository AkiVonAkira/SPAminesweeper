import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Home } from "./components/Home";
import { StartGame } from './components/StartGame';

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/start-game',
    requireAuth: true,
    element: <StartGame />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
