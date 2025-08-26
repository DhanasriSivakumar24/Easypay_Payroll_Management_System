import { createSlice } from "@reduxjs/toolkit";

const authSlice = createSlice({
  name: "auth",
  initialState: {
    username: null,
    role: null, // Add role to state
  },
  reducers: {
    login: (state, action) => {
      console.log(action.payload);
      state.username = action.payload.username;
      state.role = action.payload.role; // Set role
    },
    logout: (state) => {
      state.username = null;
      state.role = null; // Clear role on logout
    },
  },
});

export const { login, logout } = authSlice.actions;
export default authSlice.reducer;