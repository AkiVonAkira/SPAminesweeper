import React, { useState, useEffect } from "react";
import styled from "styled-components";
import axios from "axios";

const LeaderboardContainer = styled.div`
  text-align: center;
  margin: 20px auto;
  max-width: 400px;
  padding: 20px;
  border: 0.25em var(--accent) solid;
`;

const Leaderboard = () => {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isError, setIsError] = useState(false);

  useEffect(() => {
    const fetchLeaderboardData = async () => {
      try {
        const response = await axios.get("/api/score/gettopfiveoverall");
        setData(response.data);
      } catch (error) {
        console.error("Error fetching leaderboard data:", error);
        setIsError(true);
      } finally {
        setIsLoading(true);
      }
    };

    fetchLeaderboardData();
  }, []);

  return (
    <LeaderboardContainer>
      <h2>Leaderboard</h2>
      {isLoading && <p>Loading...</p>}
      {isError && <p>Error fetching data</p>}
      <ol>
        {data.map((entry, index) => (
          <li key={index}>
            Username: {entry.id}, High Score: {entry.highScore}
          </li>
        ))}
      </ol>
    </LeaderboardContainer>
  );
};

export default Leaderboard;