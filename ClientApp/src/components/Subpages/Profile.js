import { useState, useEffect } from "react";
import styled from "styled-components";
import axios from "axios";
import authService from "../api-authorization/AuthorizeService";

const ProfileContainer = styled.div`
  text-align: center;
  margin: 20px auto;
`;

const StatsContainer = styled.div`
  margin: 20px auto;
  max-width: 400px;
  padding: 20px;
  border: 0.25em var(--accent) solid;
`;

const StatItem = styled.div`
  margin-bottom: 10px;
`;

const Profile = () => {
  const [userData, setUserData] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const accessToken = await authService.getAccessToken();
        const headers = {
          Authorization: `Bearer ${accessToken}`,
        };
        const response = await axios.get("/api/user/getuser", {
          headers: headers,
        });
        console.log(response);
        setUserData(response.data);
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    };

    fetchData();
  }, []);

  return (
    <ProfileContainer>
      <h1>Players Profile</h1>
      {userData && (
        <StatsContainer>
          <h1>Stats</h1>
          <StatItem>Account: {userData.username}</StatItem>
          <StatItem>Games Played: {userData.gamesPlayed}</StatItem>
          <StatItem>Total Score: {userData.score}</StatItem>
        </StatsContainer>
      )}
    </ProfileContainer>
  );
};

export default Profile;