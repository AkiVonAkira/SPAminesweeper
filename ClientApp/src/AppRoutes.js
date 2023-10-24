import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { FetchBoard } from "./components/FetchBoard";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/fetch-board',
    requireAuth: true,
    element: <FetchBoard />
  },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;
