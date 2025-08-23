"use client";

import { motion, AnimatePresence } from "framer-motion";
import {
  Github,
  Music2,
  Sparkles,
  Play,
  Pause,
  Loader2,
  Trophy,
  Medal,
  Award,
} from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Slider } from "@/components/ui/slider"; // Import Slider
import { useState, useRef, useEffect } from "react";
import { useToast } from "@/hooks/use-toast";

export default function Home() {
  const [username, setUsername] = useState("");
  const [showPlayer, setShowPlayer] = useState(false);
  const [isPlaying, setIsPlaying] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [startDate, setStartDate] = useState("2024-01-01");
  const [endDate, setEndDate] = useState("2025-01-01");
  const [duration, setDuration] = useState(0);
  const [currentTime, setCurrentTime] = useState(0);
  const audioRef = useRef<HTMLAudioElement | null>(null);
  const { toast } = useToast();
  const apiUrl = process.env.NEXT_PUBLIC_API_URL;

  const [leaderboardData, setLeaderboardData] = useState([]);

  useEffect(() => {
    const fetchLeaderboardData = async () => {
      try {
        const response = await fetch(apiUrl + "/api/LeaderBoard?orderBy=1");
        if (!response.ok) {
          throw new Error("Failed to fetch leaderboard data");
        }
        const data = await response.json();
        setLeaderboardData(data);
      } catch (error) {
        console.error(error);
        toast({
          title: "Error",
          description: "Failed to load leaderboard data.",
          variant: "destructive",
        });
      }
    };

    fetchLeaderboardData();
  }, []);
  const getRankIcon = (rank: number) => {
    switch (rank) {
      case 1:
        return <Trophy className="w-5 h-5 text-yellow-400" />;
      case 2:
        return <Medal className="w-5 h-5 text-gray-400" />;
      case 3:
        return <Award className="w-5 h-5 text-amber-600" />;
      default:
        return (
          <span className="w-5 h-5 flex items-center justify-center text-white/60 font-bold text-sm">
            {rank}
          </span>
        );
    }
  };
  const formatTime = (seconds: number) => {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = Math.floor(seconds % 60);
    return `${minutes}:${remainingSeconds.toString().padStart(2, "0")}`;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!username) {
      toast({
        title: "Error",
        description: "Please enter a GitHub username",
        variant: "destructive",
      });
      return;
    }

    setIsLoading(true);
    try {
      var requestUrl =
        apiUrl + `/api/Music?userName=${username}&rhytmPatternType=Happy`;
      if (startDate && endDate) {
        requestUrl = `${requestUrl}&startDate=${startDate}&endDate=${endDate}`;
      }
      const response = await fetch(requestUrl, {
        method: "GET",
        headers: { "Content-Type": "application/json" },
      });

      if (!response.ok) throw new Error("Failed to generate music");
      const data = await response.json();
      const audioUrl = apiUrl + data.virtualPath;

      if (audioRef.current) {
        audioRef.current.src = audioUrl;
        audioRef.current.load(); // Ensure the new audio is loaded
        audioRef.current
          .play()
          .catch((e) => console.error("Playback failed", e));
        setIsPlaying(true);
      }

      setIsLoading(false);
      setShowPlayer(true);
      toast({
        title: "Success!",
        description: "Your music is ready to play",
      });
    } catch (error) {
      toast({
        title: "Error",
        description: "Failed to generate music. Please try again.",
        variant: "destructive",
      });
      setIsLoading(false);
    }
  };

  const togglePlay = () => {
    if (audioRef.current) {
      if (isPlaying) {
        audioRef.current.pause();
      } else {
        audioRef.current.play();
      }
      setIsPlaying(!isPlaying);
    }
  };

  const handleSeek = (value: number[]) => {
    if (audioRef.current) {
      audioRef.current.currentTime = value[0];
      setCurrentTime(value[0]);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-600 via-pink-500 to-orange-400 flex">
      {/* Leaderboard Panel */}
      <motion.div
        initial={{ x: -300, opacity: 0 }}
        animate={{ x: 0, opacity: 1 }}
        transition={{ duration: 0.8, delay: 0.2 }}
        className="w-80 bg-white/10 backdrop-blur-lg border-r border-white/20 p-6 overflow-y-auto"
      >
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-white mb-2 flex items-center gap-2">
            <Trophy className="w-6 h-6 text-yellow-400" />
            Leaderboard
          </h2>
          <p className="text-white/70 text-sm">Top commit contributors</p>
        </div>

        <div className="space-y-3">
          {leaderboardData.map((user: any, index) => (
            <motion.div
              key={user.userName}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5, delay: index * 0.1 }}
              className={`bg-white/10 backdrop-blur-sm rounded-lg p-4 border border-white/20 hover:bg-white/20 transition-all cursor-pointer ${
                user.rank <= 3 ? "ring-2 ring-yellow-400/30" : ""
              }`}
              whileHover={{ scale: 1.02 }}
              onClick={() => setUsername(user.userName)}
            >
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-3">
                  {getRankIcon(user.rank)}
                  <div>
                    <p className="text-white font-medium text-sm">
                      {user.userName}
                    </p>
                    <p className="text-white/60 text-xs">
                      {user.totalCommit} commits
                    </p>
                  </div>
                </div>
                <div className="text-right">
                  <p className="text-white/80 text-xs">#{user.rank}</p>
                </div>
              </div>
            </motion.div>
          ))}
        </div>

        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 1 }}
          className="mt-6 p-4 bg-white/5 rounded-lg border border-white/10"
        >
          <p className="text-white/60 text-xs text-center">
            Click on any user to generate their music
          </p>
        </motion.div>
      </motion.div>

      {/* Main Content */}
      <div className="flex-1 flex items-center justify-center p-4">
        <audio
          ref={audioRef}
          onLoadedMetadata={() => setDuration(audioRef.current?.duration || 0)}
          onTimeUpdate={() =>
            setCurrentTime(audioRef.current?.currentTime || 0)
          }
          onEnded={() => setIsPlaying(false)}
        />
        {/* Main Content */}
        <div className="flex-1 flex items-center justify-center p-4">
          <audio
            ref={audioRef}
            onLoadedMetadata={() =>
              setDuration(audioRef.current?.duration || 0)
            }
            onTimeUpdate={() =>
              setCurrentTime(audioRef.current?.currentTime || 0)
            }
            onEnded={() => setIsPlaying(false)}
          />
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            className="w-full max-w-md px-4"
          >
            <motion.div
              initial={{ scale: 0 }}
              animate={{ scale: 1 }}
              transition={{ type: "spring", stiffness: 260, damping: 20 }}
              className="flex justify-center mb-8 space-x-4"
            >
              <a
                href="https://www.linkedin.com/in/yunus-ozdemir/"
                target="_blank"
                rel="noopener noreferrer"
              >
                <Music2 className="w-12 h-12 text-white animate-bounce" />
              </a>
              <a
                href="https://github.com/YunusOzdemirr/CommitsToMusic"
                target="_blank"
                rel="noopener noreferrer"
              >
                <Github className="w-12 h-12 text-white animate-pulse" />
              </a>
            </motion.div>

            <motion.div
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 0.3 }}
              className="bg-white/10 backdrop-blur-lg rounded-xl p-8 shadow-xl"
            >
              <h1 className="text-3xl font-bold text-center text-white mb-2">
                Music & GitHub
              </h1>
              <p className="text-center text-white/80 mb-8">
                Enter your GitHub username to get started
              </p>

              <form onSubmit={handleSubmit} className="space-y-6">
                <div className="relative">
                  <motion.div
                    whileHover={{ scale: 1.02 }}
                    whileTap={{ scale: 0.98 }}
                  >
                    <Input
                      type="text"
                      placeholder="GitHub Username"
                      value={username}
                      onChange={(e) => setUsername(e.target.value)}
                      className="bg-white/20 border-white/30 text-white placeholder:text-white/50 focus:ring-2 focus:ring-white/50"
                      disabled={isLoading}
                    />
                  </motion.div>
                  <Sparkles className="absolute right-3 top-1/2 transform -translate-y-1/2 text-white/50 w-5 h-5" />
                </div>

                <div className="relative">
                  <Input
                    type="date"
                    placeholder="Start Date"
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    className="bg-white/20 border-white/30 text-white placeholder:text-white/50 focus:ring-2 focus:ring-white/50"
                    disabled={isLoading}
                  />
                </div>

                <div className="relative">
                  <Input
                    type="date"
                    placeholder="End Date"
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    className="bg-white/20 border-white/30 text-white placeholder:text-white/50 focus:ring-2 focus:ring-white/50"
                    disabled={isLoading}
                  />
                </div>

                <motion.div
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                >
                  <Button
                    type="submit"
                    className="w-full bg-white text-purple-600 hover:bg-white/90 transition-all disabled:opacity-50"
                    disabled={isLoading}
                  >
                    {isLoading ? (
                      <>
                        <Loader2 className="w-5 h-5 mr-2 animate-spin" />
                        Generating Music...
                      </>
                    ) : (
                      <>
                        <Github className="w-5 h-5 mr-2" />
                        Submit
                      </>
                    )}
                  </Button>
                </motion.div>
              </form>

              <AnimatePresence>
                {showPlayer && (
                  <motion.div
                    initial={{ opacity: 0, height: 0 }}
                    animate={{ opacity: 1, height: "auto" }}
                    exit={{ opacity: 0, height: 0 }}
                    transition={{ duration: 0.5 }}
                    className="mt-6 bg-white/20 rounded-lg p-4 shadow-lg"
                  >
                    <div className="flex items-center justify-between">
                      <div className="text-white font-medium">
                        Your GitHub Music
                      </div>
                      <Button
                        onClick={togglePlay}
                        variant="ghost"
                        size="icon"
                        className="text-white hover:bg-white/20"
                      >
                        {isPlaying ? (
                          <Pause className="w-6 h-6" />
                        ) : (
                          <Play className="w-6 h-6" />
                        )}
                      </Button>
                    </div>
                    <div className="mt-2">
                      <Slider
                        value={[currentTime]}
                        max={duration}
                        step={1}
                        onValueChange={handleSeek}
                        className="w-full"
                      />
                      <div className="flex justify-between text-xs text-white/80 mt-1">
                        <span>{formatTime(currentTime)}</span>
                        <span>{formatTime(duration)}</span>
                      </div>
                    </div>
                  </motion.div>
                )}
              </AnimatePresence>
            </motion.div>

            <motion.div
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 0.6 }}
              className="mt-8 text-center text-white/60 text-sm"
            >
              Discover the harmony between code and music
            </motion.div>

            <motion.div
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 0.6 }}
              className="mt-8 text-center text-white/60 text-sm flex gap-1 flex-col"
            >
              <p className="footer-text">Made From Yunus Özdemir</p>
              <p className="footer-text">Made By .NET 9 and Nextjs</p>
              <div className="flex gap-1 justify-center">
                <h3>Reach Me From: </h3>
                <a
                  href="https://www.youtube.com/@yunusozdemirs"
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-white"
                >
                  YouTube
                </a>
                <a
                  href="https://www.linkedin.com/in/yunus-ozdemir"
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-white"
                >
                  LinkedIn
                </a>
                <a
                  href="https://github.com/YunusOzdemirr"
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-white"
                >
                  GitHub
                </a>
              </div>
            </motion.div>
          </motion.div>
        </div>
      </div>
    </div>
  );
}
