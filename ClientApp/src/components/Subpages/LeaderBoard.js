import React, { Component, useState, useEffect } from "react";
import authService from "../api-authorization/AuthorizeService";
import styled from "styled-components";
import axios from "axios";

const LeaderboardContainer = styled.div`
  justify-content: center;
  text-align: left;
  max-width: 100%;
  border-radius: 0.5em;
  width: 100%;
  padding: 1em;
`;
const LeaderboardH2 = styled.h2`
  text-align: center;
`;

// export class Leaderboard extends Component {
//   constructor(props) {
//     super(props);
//     this.state = { leaderboard: null, loading: false };
//   }

//   async componentDidMount() {
//     await this.updateTopFive();
//   }

//   async updateTopFive() {
//     this.setState({ loading: true });

//     try {
//       const token = await authService.getAccessToken();
//       const isDaily = false;
//       const response = await axios.get(`/api/score/gettopfivescores?isDaily=${isDaily}`, null, {
//         headers: {
//           Authorization: token ? `Bearer ${token}` : ""
//         }
//       });

//       if (response.status >= 200 && response.status < 300) {
//         this.setState({ leaderboard: response.data, loading: false });
//       } else if (response.status > 400 && response.status < 500) {
//         this.setState({ loading: true });
//       } else {
//         console.error("Failed to fetch leaderboard data", response);
//       }
//     } catch (error) {
//       console.error("Error fetching leaderboard data", error);
//     } finally {
//       this.setState({ loading: false });
//     }
//   }

//   renderLeaderboard() {
//     const { leaderboard } = this.state;

//     if (!leaderboard) {
//       return <p>Loading...</p>;
//     }

//     return (
//       <LeaderboardContainer>
//         <h2>Leaderboard</h2>
//         <ol>
//           {leaderboard.map((entry, index) => (
//             <li key={index}>
//               Username: {entry.id}, High Score: {entry.highScore}
//             </li>
//           ))}
//         </ol>
//       </LeaderboardContainer>
//     );
//   }

//   render() {
//     return this.renderLeaderboard();
//   }
// }



const Leaderboard = () => {
  const [leaderboardData, setLeaderboardData] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const accessToken = await authService.getAccessToken();
        const isDaily = false;
        const response = await axios.get("/api/score/gettopfivescores", {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
          }
        });
        //console.log(response);
        setLeaderboardData(response.data);
      } catch (error) {
        console.error("Error fetching leaderboard data:", error);
      }
    };

    fetchData();
  }, []);

  return (
    <LeaderboardContainer>
      <LeaderboardH2>Leaderboard</LeaderboardH2>
      {leaderboardData && (
        <ol>
          {
            leaderboardData.map((entry, index) => (
              <li key={index}>
                {entry.username} - High Score: {entry.highScore}
              </li>
            ))
          }
        </ol>
      )}
    </LeaderboardContainer>
  );
};

export default Leaderboard;
