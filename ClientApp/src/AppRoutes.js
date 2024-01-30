import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { Home } from "./components/Homepage/Home";
import { StartGame } from './components/Subpages/StartGame';

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
