import { useState, useEffect } from "react";
import styled from "styled-components";
import axios from "axios";
import authService from "../api-authorization/AuthorizeService";

const ProfileContainer = styled.div`
  text-align: center;
  padding: 1em;
  width: 100%;
`;

const StatsContainer = styled.div`
  margin: 1em;
  max-width: 400px;
  text-align: left;
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
        const response = await axios.get("/api/user/getuser", {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${accessToken}`,
          }
        });
        //console.log(response);
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
          <StatItem><b>Player:</b> {userData.username}</StatItem>
          <StatItem><b>Games Played:</b> {userData.gamesPlayed}</StatItem>
          {/* <StatItem><b>Total Score:</b> {userData.highScore}</StatItem>
          <StatItem><b>Highest Score:</b> {userData.score}</StatItem> */}
        </StatsContainer>
      )}
    </ProfileContainer>
  );
};

export default Profile;