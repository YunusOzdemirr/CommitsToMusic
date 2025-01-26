"use client";

import { motion, AnimatePresence } from "framer-motion";
import { Github, Music2, Sparkles, Play, Pause, Loader2 } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useState, useRef } from "react";
import { useToast } from "@/hooks/use-toast";

export default function Home() {
  const [username, setUsername] = useState("");
  const [startDate, setStartDate] = useState(""); // Start Date state
  const [endDate, setEndDate] = useState(""); // End Date state
  const [showPlayer, setShowPlayer] = useState(false);
  const [isPlaying, setIsPlaying] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const audioRef = useRef<HTMLAudioElement | null>(null);
  const { toast } = useToast();
  const apiUrl = "https://commitstomusic.com.tr";

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!username || !startDate || !endDate) {
      toast({
        title: "Error",
        description: "Please fill all fields",
        variant: "destructive",
      });
      return;
    }

    setIsLoading(true);
    try {
      //API REQUEST: Send GitHub username and dates to backend
      const response = await fetch(
        `${apiUrl}/api/Music?userName=${username}&startDate=${startDate}&endDate=${endDate}&rhytmPatternType=Sad`,
        {
          method: "GET",
          headers: { "Content-Type": "application/json" },
        }
      );

      if (!response.ok) throw new Error("Failed to generate music");

      const data = await response.json();
      const audioUrl = apiUrl + data.virtualPath;

      if (audioRef.current) {
        audioRef.current.src = audioUrl;
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

  return (
    <div className="min-h-screen bg-gradient-to-br from-purple-600 via-pink-500 to-orange-400 flex items-center justify-center">
      <audio ref={audioRef} />
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
          <Music2 className="w-12 h-12 text-white animate-bounce" />
          <Github className="w-12 h-12 text-white animate-pulse" />
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
            Enter your GitHub username and date range
          </p>

          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="relative">
              <Input
                type="text"
                placeholder="GitHub Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="bg-white/20 border-white/30 text-white placeholder:text-white/50 focus:ring-2 focus:ring-white/50"
                disabled={isLoading}
              />
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
          </form>
        </motion.div>
      </motion.div>
    </div>
  );
}
